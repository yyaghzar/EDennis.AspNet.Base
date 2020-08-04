﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EDennis.NetStandard.Base {

    /// <summary>
    /// Base proxy ("external") controller for communicating with an
    /// internal controller via secure HttpClient.
    /// Note: setup the DependencyInjection to include:
    /// <list type="bullet">
    /// <item>AddHttpClient("{name of the controller class}", config=> { config.BaseAddress = ... ;});</item>
    /// <item>AddSingleton<ITokenService,ClientCredentialsTokenService>(); //or use MockTokenService to bypass token generation</item>
    /// </list>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class ProxyCrudController<TEntity> : ProxyQueryController<TEntity>, ICrudController<TEntity>
        where TEntity : class, ICrudEntity {


        public ProxyCrudController(IHttpClientFactory clientFactory, ITokenService tokenService) :
            base(clientFactory,tokenService) { }


        [HttpPost]
        public IActionResult Create([FromBody] TEntity input) {
            return _client.Post($"{ControllerPath}",input);
        }

        [HttpPost("async")]
        public async Task<IActionResult> CreateAsync([FromBody] TEntity input) {
            return await _client.PostAsync($"{ControllerPath}/async", input);
        }

        [HttpDelete("{**key}")]
        public IActionResult Delete([FromRoute] string key) {
            return _client.Delete<TEntity>($"{ControllerPath}/{key}");
        }

        [HttpDelete("async/{**key}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] string key) {
            return await _client.DeleteAsync<TEntity>($"{ControllerPath}/{key}");
        }

        [NonAction]
        public IQueryable<TEntity> Find(string pathParameter) {
            throw new System.NotImplementedException();
        }


        [HttpGet("{**key}")]
        public IActionResult GetById([FromRoute] string key) {
            return _client.Get<TEntity>($"{ControllerPath}/{key}");
        }


        [HttpGet("async/{**key}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string key) {
            return await _client.GetAsync<TEntity>($"{ControllerPath}/{key}");
        }


        [HttpPatch("{**key}")]
        public IActionResult Patch([FromRoute] string key, [FromBody] JsonElement input) {
            return _client.Patch($"{ControllerPath}/{key}", input);
        }


        [HttpPatch("async/{**key}")]
        public async Task<IActionResult> PatchAsync([FromRoute] string key, [FromBody] JsonElement input) {
            return await _client.PatchAsync($"{ControllerPath}/{key}", input);
        }


        [HttpPut("{**key}")]
        public IActionResult Update([FromRoute] string key, [FromBody] TEntity input) {
            return _client.Put($"{ControllerPath}/{key}", input);
        }

        [HttpPut("async/{**key}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string key, [FromBody] TEntity input) {
            return await _client.PutAsync($"{ControllerPath}/{key}", input);
        }
    }
}
