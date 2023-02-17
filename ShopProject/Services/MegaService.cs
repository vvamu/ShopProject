using CG.Web.MegaApiClient;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Drawing;
using ShopProject.Helper;

namespace ShopProject.Services
{
    public class MegaService
    {
        private IConfiguration _configuration;
        private static IEnumerable<INode> nodes;



        private static string _uriParentFolder = "https://mega.nz/folder/El80WQIR#I4rsPoud7SzFOqBK9LtZDw";

        public MegaService()
        {

            //MegaApiClient client = new MegaApiClient();
            //client.Login(configuration.GetConnectionString("MegaUsername"),
            //    configuration.GetConnectionString("MegaPassword"));


            //client.Login("6183866@gmail.com",
            //    "2002y1f1l1z1");


            //IEnumerable<INode> allNodes = client.GetNodes();
            //parentNode = allNodes.SingleOrDefault(m => m.Name == "ShopProject" && m.Type == NodeType.Directory);

            //"https://mega.nz/folder/El80WQIR#I4rsPoud7SzFOqBK9LtZDw"
            //nodes = allNodes.Where(x => x.ParentId == parentNode.Id && x.Type == NodeType.File) ?? new List<INode>();




            //GetImageByUrl("fe.jpg");



            //client.Logout();

        }


        public static async Task<byte[]> GetModelImage(Data.Help.ApplicationClass model)
        {

            //Console.WriteLine($"Downloading {node.Name}");
            //client.DownloadFile(fileLink, node.Name);

            //client.Logout();

            MegaApiClient client = new MegaApiClient();
            client.LoginAnonymous();

            Uri fileLink;
            if (model.ImageUri != null) fileLink = model.ImageUri;
            else if (model.ImagePath != null) fileLink = new Uri(model.ImagePath);
            else { return null; }
                INode node = client.GetNodeFromLink(fileLink);


            var stream = await client.DownloadAsync(node);
            var bytes = new byte[stream.Length];

            await stream.ReadAsync(bytes);
            await client.LogoutAsync();



            return bytes;
            

        }

        public static async Task<bool> UploadImage(Data.Help.ApplicationClass model, IFormFile image)
        {
            foreach (var type in Enum.GetNames<ImageType>())
            {
                if (image.FileName.Contains(type))
                {
                    MegaApiClient client = new MegaApiClient();
                    client.Login("6183866@gmail.com","2002y1f1l1z1");

                    //INode root = client.GetNodeFromLink(new Uri(_uriParentFolder));
                    INode root = client.GetNodes().Single(x => x.Name == "ShopProject");
                    //INode myFolder = client.CreateFolder("Upload", root);

                    INode myFile = await client.UploadAsync(image.OpenReadStream(),image.FileName, root);
                    model.ImageUri = await client.GetDownloadLinkAsync(myFile);

                    await client.LogoutAsync();
                    

                    return true;

                }
            }
            return false;
        }
        void DisplayNodesRecursive(IEnumerable<INode> nodes, INode parent, int level = 0)
        {
            IEnumerable<INode> children = nodes.Where(x => x.ParentId == parent.Id);
            foreach (INode child in children)
            {
                string infos = $"- {child.Name} - {child.Size} bytes - {child.CreationDate}";
                Console.WriteLine(infos.PadLeft(infos.Length + level, '\t'));
                if (child.Type == NodeType.Directory)
                {
                    DisplayNodesRecursive(nodes, child, level + 1);
                }
            }
        }

    }
}
