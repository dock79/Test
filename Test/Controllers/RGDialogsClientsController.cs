using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Test.Models;


namespace Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RGDialogsClientsController : ControllerBase
    {
        private List<RGDialogsClients> _rgDialogsClientsList;

        public RGDialogsClientsController()
        {
            RGDialogsClients rgDialogsClients = new RGDialogsClients();
            _rgDialogsClientsList = rgDialogsClients.Init();
        }

        
        /// <summary>
        /// Метод возвращается идентификатор первого найденного диалога 
        /// </summary>
        /// <param name="guidsListc">Список идентификаторов клиентов</param>
        [Route("dialog")]
        [HttpGet]
        [Produces(typeof(Guid))]
        public IActionResult GetDialog([FromQuery] List<string> guidsList)
        {
            if ((guidsList is null) || (!guidsList.All(s => Guid.TryParse(s, out _))))
                return BadRequest("Ошибка. Входные данные не корректны.");

            List<Guid> guids = guidsList.ConvertAll(Guid.Parse);

            // Получение Dictionary у которого Key равно ID диалога, а Value список ID всех клиентов в диалоге 
            Dictionary<Guid, List<Guid>> dict = _rgDialogsClientsList
                .GroupBy(rgddialogClients => rgddialogClients.IDRGDialog)
                .ToDictionary(dialogs => dialogs.Key, grp => grp.Select(clients => clients.IDClient).ToList());

            List<Guid> resultList = new List<Guid>();

            //Нахождение входит ли переданный список клиентов в какой-то диалог 
            foreach (KeyValuePair<Guid, List<Guid>> entry in dict)
            {
                var guid = entry.Key;
                var list = entry.Value;
                bool result = !guids.Except(list).Any();
                if (result) 
                {
                    resultList.Add(guid);
                    return Ok(resultList);
                }
            }

            if (resultList.Count == 0) resultList.Add(Guid.Empty);

            return Ok(resultList);
        }
        
        
        /// <summary>
        /// Метод возвращается идентификатор всех найденных диалогов
        /// </summary>
        /// <param name="guidsListc">Список идентификаторов клиентов</param>
        [Route("dialogs")]
        [HttpGet]
        [Produces(typeof(List<Guid>))]
        public List<Guid> Get([FromQuery] List<Guid> guids)
        {
            // Проверка корректности входных данных
            List<Guid> resultList = new List<Guid>();

            if ((guids is null) || (!guids.All(g => Guid.TryParse(g.ToString(), out _))))
            {
                resultList.Add(Guid.Empty);
                return resultList;
            }

            // Получение Dictionary у которого Key равно ID диалога, а Value список ID всех клиентов в диалоге 
            Dictionary<Guid, List<Guid>> dict = _rgDialogsClientsList
                .GroupBy(rgddialogClients => rgddialogClients.IDRGDialog)
                .ToDictionary(dialogs => dialogs.Key, grp => grp.Select(clients => clients.IDClient).ToList());

            //Нахождение входит ли переданный список клиентов в какой-то диалог 
            foreach (KeyValuePair<Guid, List<Guid>> entry in dict)
            {
                var guid = entry.Key;
                var list = entry.Value;
                bool result = !guids.Except(list).Any();
                if (result)
                {
                    resultList.Add(guid);
                }
            }

            if (resultList.Count == 0) resultList.Add(Guid.Empty);

            return resultList;
        }

        /// <summary>
        /// Метод возвращается идентификатор всех найденных диалогов  
        /// </summary>
        /// <param name="guidsListc">Список идентификаторов клиентов</param>
        [Route("dialogsversion2")]
        [HttpGet]
        [Produces(typeof(List<Guid>))]
        public IActionResult Get([FromQuery] List<string> guidsList)
        {
            
            if ((guidsList is null) || (!guidsList.All(s => Guid.TryParse(s, out var newGuid))))
                return BadRequest("Ошибка. Входные данные не корректны.");

            List<Guid> guids = guidsList.ConvertAll(Guid.Parse);

            // Получение Dictionary у которого Key равно ID диалога, а Value список ID всех клиентов в диалоге 
            Dictionary<Guid, List<Guid>> dict = _rgDialogsClientsList
                .GroupBy(rgddialogClients => rgddialogClients.IDRGDialog)
                .ToDictionary(dialogs => dialogs.Key, grp => grp.Select(clients => clients.IDClient).ToList());

            List<Guid> resultList = new List<Guid>();

            //Нахождение входит ли переданный список клиентов в какой-то диалог 
            foreach (KeyValuePair<Guid, List<Guid>> entry in dict)
            {
                var guid = entry.Key;
                var list = entry.Value;
                bool result = !guids.Except(list).Any();
                if (result)
                {
                    resultList.Add(guid);
                }
            }

            if (resultList.Count == 0) resultList.Add(Guid.Empty);

            return Ok(resultList);
        }
        
        
        
        
        
    }
}