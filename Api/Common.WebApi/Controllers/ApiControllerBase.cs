using Common.WebApi.ConnectionsDb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace Common.WebApi.Controllers
{
    public class ApiControllerBase: ControllerBase
    {
        public ApiControllerBase()
        {

        }
    }
}
