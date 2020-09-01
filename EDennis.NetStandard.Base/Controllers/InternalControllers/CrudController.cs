﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace EDennis.NetStandard.Base {
    [Route(ApiConstants.ROUTE_PREFIX + "[controller]")]
    [ApiController]
    public abstract class CrudController<TContext, TEntity> : QueryController<TContext, TEntity>, ICrudController<TEntity> where TContext : DbContext
        where TEntity : class, ICrudEntity {

        protected string _sysUser;

        public const int UNPROCESSABLE_ENTITY_STATUS_CODE = 422;
        public const int LOCKED_STATUS_CODE = 423;


        [NonAction]
        protected void SetSysUser(TEntity entity) {
            _sysUser ??= HttpContext.User.Claims
                .OrderByDescending(c => c.Type)
                .FirstOrDefault(c => c.Type == "name"
                    || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
                    || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/email"
                    || c.Type == "email"
                    || c.Type == "client_id")
                ?.Value;
            entity.SysUser = _sysUser;
        }


        #region Overrideable Methods

        [NonAction]
        public abstract IQueryable<TEntity> Find(string pathParameter);

        [NonAction]
        protected virtual void BeforeCreate(TEntity input) { }
        [NonAction]
        protected virtual void BeforeUpdate(TEntity existing) { }
        [NonAction]
        protected virtual void BeforeDelete(TEntity existing) { }

        [NonAction]
        protected virtual void DoCreate(TEntity input) => _dbContext.Add(input);
        [NonAction]
        protected virtual void DoUpdate(TEntity input, TEntity existing) => existing.Update(input);
        [NonAction]
        protected virtual void DoPatch(JsonElement input, TEntity existing) => existing.Patch(input, ModelState);
        [NonAction]
        protected virtual void DoDelete(TEntity existing) => _dbContext.Remove(existing);


        #endregion


        public CrudController(DbContextProvider<TContext> provider,
            ILogger<QueryController<TContext, TEntity>> logger) : base(provider, logger) {

        }




        [HttpGet("{**key}")]
        public virtual IActionResult GetById([FromRoute] string key) {
            var qry = Find(key);
            AdjustQuery(ref qry);
            var entity = qry.FirstOrDefault();

            if (entity == null)
                return NotFound();
            else
                return Ok(entity);
        }


        [HttpGet("async/{**key}")]
        public virtual async Task<IActionResult> GetByIdAsync([FromRoute] string key) {
            var qry = Find(key);
            AdjustQuery(ref qry);
            var entity = await qry.FirstOrDefaultAsync();

            if (entity == null)
                return NotFound();
            else
                return Ok(entity);
        }





        [HttpPost]
        public virtual IActionResult Create([FromBody] TEntity input) {

            SetSysUser(input);
            BeforeCreate(input);
            DoCreate(input);

            try {
                _dbContext.SaveChanges();
                return Ok(input);
            } catch (DbUpdateException ex) {
                using (_logger.BeginScope(GetLoggerScope(input)))
                    _logger.LogError(ex.Message);
                ModelState.AddModelError("", $"An instance of {typeof(TEntity).Name} could not be created with values: {input}");
                return Conflict(ModelState);
            }
        }

        [HttpPost("async")]
        public virtual async Task<IActionResult> CreateAsync([FromBody] TEntity input) {

            SetSysUser(input);
            BeforeCreate(input);
            DoCreate(input);

            try {
                await _dbContext.SaveChangesAsync();
                return Ok(input);
            } catch (DbUpdateException ex) {
                using (_logger.BeginScope(GetLoggerScope(input)))
                    _logger.LogError(ex.Message);
                ModelState.AddModelError("", $"An instance of {typeof(TEntity).Name} could not be created with values: {input}");
                return Conflict(ModelState);
            }
        }


        [HttpPut("{**key}")]
        public virtual IActionResult Update([FromRoute] string key, [FromBody] TEntity input) {

            //retrieve the existing entity
            var existing = Find(key).FirstOrDefault();

            //check NotFound, Gone (deleted), Locked
            if (NotFoundGoneLocked(existing, out IActionResult result))
                return result;

            BeforeUpdate(existing);
            DoUpdate(input, existing);
            SetSysUser(existing);

            try {
                _dbContext.SaveChanges();
                return Ok(existing);
            } catch (DbUpdateConcurrencyException ex2) {
                using (_logger.BeginScope(GetLoggerScope(input)))
                    _logger.LogError(ex2.Message);
                ModelState.AddModelError("", ex2.Message);
                return new ObjectResult(ModelState) { StatusCode = (int)HttpStatusCode.Conflict };
            } catch (DbUpdateException ex1) {
                using (_logger.BeginScope(GetLoggerScope(input)))
                    _logger.LogError(ex1.Message);
                ModelState.AddModelError("", ex1.InnerException.Message);
                return new ObjectResult(ModelState) { StatusCode = (int)HttpStatusCode.Conflict };
            }
        }


        [HttpPut("async/{**key}")]
        public virtual async Task<IActionResult> UpdateAsync([FromRoute] string key, [FromBody] TEntity input) {

            //retrieve the existing entity
            var existing = await Find(key).FirstOrDefaultAsync();

            //check NotFound, Gone (deleted), Locked
            if (NotFoundGoneLocked(existing, out IActionResult result))
                return result;

            BeforeUpdate(existing);
            DoUpdate(input, existing);
            SetSysUser(existing);

            try {
                await _dbContext.SaveChangesAsync();
                return Ok(existing);
            } catch (DbUpdateConcurrencyException ex2) {
                using (_logger.BeginScope(GetLoggerScope(input)))
                    _logger.LogError(ex2.Message);
                ModelState.AddModelError("", ex2.Message);
                return new ObjectResult(ModelState) { StatusCode = (int)HttpStatusCode.Conflict };
            } catch (DbUpdateException ex1) {
                using (_logger.BeginScope(GetLoggerScope(input)))
                    _logger.LogError(ex1.Message);
                ModelState.AddModelError("", ex1.InnerException.Message);
                return new ObjectResult(ModelState) { StatusCode = (int)HttpStatusCode.Conflict };
            }
        }


        [HttpPatch("{**key}")]
        public virtual IActionResult Patch([FromRoute] string key, [FromBody] JsonElement input) {

            if (input.ValueKind != JsonValueKind.Object)
                return new ObjectResult($"Cannot update {typeof(TEntity).Name} with {input.GetRawText().Substring(0, 200) + "..."}") { StatusCode = UNPROCESSABLE_ENTITY_STATUS_CODE };

            //retrieve the existing entity
            var existing = Find(key).FirstOrDefault();

            //check NotFound, Gone (deleted), Locked
            if (NotFoundGoneLocked(existing, out IActionResult result))
                return result;

            BeforeUpdate(existing);
            DoPatch(input, existing);
            SetSysUser(existing);//must be after, when a patch

            try {
                _dbContext.SaveChanges();
                return Ok(existing);
            } catch (DbUpdateConcurrencyException ex2) {
                using (_logger.BeginScope(GetLoggerScope(input)))
                    _logger.LogError(ex2.Message);
                ModelState.AddModelError("", ex2.Message);
                return new ObjectResult(ModelState) { StatusCode = (int)HttpStatusCode.Conflict };
            } catch (DbUpdateException ex1) {
                using (_logger.BeginScope(GetLoggerScope(input)))
                    _logger.LogError(ex1.Message);
                ModelState.AddModelError("", ex1.InnerException.Message);
                return new ObjectResult(ModelState) { StatusCode = (int)HttpStatusCode.Conflict };
            }
        }


        [HttpPatch("async/{**key}")]
        public virtual async Task<IActionResult> PatchAsync([FromRoute] string key, [FromBody] JsonElement input) {

            if (input.ValueKind != JsonValueKind.Object)
                return new ObjectResult($"Cannot update {typeof(TEntity).Name} with {input.GetRawText().Substring(0, 200) + "..."}") { StatusCode = UNPROCESSABLE_ENTITY_STATUS_CODE };

            //retrieve the existing entity
            var existing = await Find(key).FirstOrDefaultAsync();

            //check NotFound, Gone (deleted), Locked
            if (NotFoundGoneLocked(existing, out IActionResult result))
                return result;

            BeforeUpdate(existing);
            DoPatch(input, existing);
            SetSysUser(existing);//must be after, when a patch

            try {
                await _dbContext.SaveChangesAsync();
                return Ok(existing);
            } catch (DbUpdateConcurrencyException ex2) {
                using (_logger.BeginScope(GetLoggerScope(input)))
                    _logger.LogError(ex2.Message);
                ModelState.AddModelError("", ex2.Message);
                return new ObjectResult(ModelState) { StatusCode = (int)HttpStatusCode.Conflict };
            } catch (DbUpdateException ex1) {
                using (_logger.BeginScope(GetLoggerScope(input)))
                    _logger.LogError(ex1.Message);
                ModelState.AddModelError("", ex1.InnerException.Message);
                return new ObjectResult(ModelState) { StatusCode = (int)HttpStatusCode.Conflict };
            }
        }

        [HttpDelete("{**key}")]
        public virtual IActionResult Delete([FromRoute] string key) {

            var existing = Find(key).FirstOrDefault();

            //check NotFound, Gone (deleted), Locked
            if (NotFoundGoneLocked(existing, out IActionResult result))
                return result;

            BeforeDelete(existing);
            DoDelete(existing);

            try {
                _dbContext.SaveChanges();
            } catch (DbUpdateConcurrencyException ex2) {
                using (_logger.BeginScope(GetLoggerScope(new { key })))
                    _logger.LogError(ex2.Message);
                ModelState.AddModelError("", $"Delete of { typeof(TEntity).Name }({key}) created concurrency conflict: {ex2.InnerException.Message}");
                return new ObjectResult(ModelState) { StatusCode = (int)HttpStatusCode.Conflict };
            } catch (DbUpdateException ex1) {
                using (_logger.BeginScope(GetLoggerScope(new { key })))
                    _logger.LogError(ex1.Message);
                ModelState.AddModelError("", $"Could not delete { typeof(TEntity).Name }({key}): {ex1.InnerException.Message}");
                return new ObjectResult(ModelState) { StatusCode = (int)HttpStatusCode.Conflict };
            }

            return NoContent();
        }


        [HttpDelete("async/{**key}")]
        public async virtual Task<IActionResult> DeleteAsync([FromRoute] string key) {

            var existing = await Find(key).FirstOrDefaultAsync();

            //check NotFound, Gone (deleted), Locked
            if (NotFoundGoneLocked(existing, out IActionResult result))
                return result;

            BeforeDelete(existing);
            DoDelete(existing);

            try {
                await _dbContext.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException ex2) {
                using (_logger.BeginScope(GetLoggerScope(new { key })))
                    _logger.LogError(ex2.Message);
                ModelState.AddModelError("", $"Delete of { typeof(TEntity).Name }({key}) created concurrency conflict: {ex2.InnerException.Message}");
                return new ObjectResult(ModelState) { StatusCode = (int)HttpStatusCode.Conflict };
            } catch (DbUpdateException ex1) {
                using (_logger.BeginScope(GetLoggerScope(new { key })))
                    _logger.LogError(ex1.Message);
                ModelState.AddModelError("", $"Could not delete { typeof(TEntity).Name }({key}): {ex1.InnerException.Message}");
                return new ObjectResult(ModelState) { StatusCode = (int)HttpStatusCode.Conflict };
            }

            return NoContent();
        }


        private bool NotFoundGoneLocked(TEntity entity, out IActionResult result) {

            result = null;
            if (entity != null && entity.SysStatus != SysStatus.Deleted && entity.SysStatus != SysStatus.Locked)
                return false;

            if (entity == null) {
                using (_logger.BeginScope(GetLoggerScope(entity)))
                    _logger.LogWarning("{Entity} record could not be found", entity.GetType().Name);
                ModelState.AddModelError("", $"The {entity.GetType().Name} record could not be found: {entity}");
                result = NotFound(ModelState);
                return true;
            } else if (entity.SysStatus == SysStatus.Deleted) {
                using (_logger.BeginScope(GetLoggerScope(entity)))
                    _logger.LogWarning("{Entity} record was deleted", entity.GetType().Name);
                ModelState.AddModelError("", $"The {entity.GetType().Name} record was deleted: {entity}");
                result = new ObjectResult(ModelState) { StatusCode = (int)HttpStatusCode.Gone };
                return true;
            } else if (entity.SysStatus == SysStatus.Locked) {
                using (_logger.BeginScope(GetLoggerScope(entity)))
                    _logger.LogWarning("{Entity} record is locked", entity.GetType().Name);
                ModelState.AddModelError("", $"The {entity.GetType().Name} record is locked: {entity}");
                result = new ObjectResult(ModelState) { StatusCode = LOCKED_STATUS_CODE };
                return true;
            } else
                return false;
        }


    }
}
