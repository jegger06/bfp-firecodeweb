using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EBFP.WebAPI.Controllers
{
    [RoutePrefix("api/EstablishmentServices")]

    public class EstablishmentServicesController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Index(string encryptedId)
        {
            try
            {
                var model = new TestModel();
                return Json(model);
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception exception)
            {
                //ExceptionHelper.ThrowHttpResponseException(exception);
                throw;
            }
        }

        [HttpGet]
        [Route("GetEstablishmentById/{encryptedId}")]
        public IHttpActionResult GetEstablishmentById(string encryptedId)
        {
            try
            {
                var model = new TestModel();
                return Json(model);
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception exception)
            {
                //ExceptionHelper.ThrowHttpResponseException(exception);
                throw;
            }
        }
    }

    public class TestModel
    {
        public string Name { get; set; }
    }
}
