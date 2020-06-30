using PETSHOP.Common;
using PETSHOP.Models;
using PETSHOP.Models.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PETSHOP.Utils
{
    public static class GetApiInventoryReceiveNotes
    {
        public static IEnumerable<InventoryReceivingNote> GetInventoryReceivingNotes(string token)
        {
            IEnumerable<InventoryReceivingNote> res = null;
            using (var client = HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Constants.BASE_URI);
                var responseTask = client.GetAsync(Constants.INVENTORY_RECEIVE_NOTE);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<InventoryReceivingNote>>();
                    readTask.Wait();

                    res = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    res = Enumerable.Empty<InventoryReceivingNote>();
                }
            }

            return res;
        }

        public static InventoryReceivingNote Post(InventoryReceiveNoteModel inventoryReceivingNote, string token)
        {
            using (var client = HelperClient.GetClient(token))
            {
                client.BaseAddress = new Uri(Common.Constants.BASE_URI);

                var postTask = client.PostAsJsonAsync<InventoryReceiveNoteModel>(Constants.INVENTORY_RECEIVE_NOTE, inventoryReceivingNote);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<InventoryReceivingNote>();
                    readTask.Wait();

                    return readTask.Result;
                }
                return null;
            }
        }
    }
}
