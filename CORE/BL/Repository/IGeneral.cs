using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeicipFranLabERP.BL.Repository
{
    //this is made for the repetative actions
  public  interface IGeneral
    {
        string SaveImages(IFormFile received_file, string destination_folders);
        string GetFileLocationWithWebRootPath(string file_new_name);
    }
}
