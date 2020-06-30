using PETSHOP.Common;
using PETSHOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiInventoryReceiveNoteDetailForToys
    {
        public static IEnumerable<InventoryReceivingNoteDetailForToy> GetInventoryReceivingNoteDetailForToys(string token)
        {
            IEnumerable<InventoryReceivingNoteDetailForToy> res = null;
            using (var client = HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync(Constants.INVENTORY_RECEIVE_NOTE_TOY);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<InventoryReceivingNoteDetailForToy>>();
                    readTask.Wait();

                    res = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    res = Enumerable.Empty<InventoryReceivingNoteDetailForToy>();
                }
            }

            return res;
        }

        public static InventoryReceivingNoteDetailForToy Post(
            InventoryReceivingNoteDetailForToy inventoryReceivingNoteDetailForToy,
            string token
            )
        {
            using (var client = HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var postTask = client
                    .PostAsJsonAsync<InventoryReceivingNoteDetailForToy>(Constants.INVENTORY_RECEIVE_NOTE_FOOD, inventoryReceivingNoteDetailForToy);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<InventoryReceivingNoteDetailForToy>();
                    readTask.Wait();

                    return readTask.Result;
                }
                return null;
            }
        }
    }
}
