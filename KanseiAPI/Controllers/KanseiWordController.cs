using KanseiAPI.Interface;
using KanseiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KanseiAPI.Controllers
{
    [Route("api/kansei-word")]
    [ApiController]
    public class KanseiWordController
    {
        private IKanseiWordRepository kanseiWordRepository;
        private List<Kansei> kanseiWords;
        private List<Kansei> kanseiPreprocess;
        public KanseiWordController(IKanseiWordRepository kanseiWordRepository)
        {
            this.kanseiWordRepository = kanseiWordRepository;
            kanseiWords = new List<Kansei>();
            kanseiPreprocess = new List<Kansei>();
        }

        [HttpGet("", Name = "GetAllKanseiWord")]
        public async Task<ActionResult<ResponseInfo>> getAll()
        {
            ResponseInfo response = new ResponseInfo();
            try
            {
                response.statusCode = System.Net.HttpStatusCode.OK;
                response.data = kanseiPreprocess;
                return await Task.FromResult(response);

            }
            catch (Exception e)
            {
                response.statusCode = System.Net.HttpStatusCode.BadRequest;
                return await Task.FromResult(response);
            }
        }

        private static readonly List<string> type = new List<string>() { "TL01", "TL02", "TL03" };

        [HttpPost("", Name = "PostNewKansei")]
        public async Task<ActionResult<ResponseInfo>> addKansei(List<Kansei> listKansei)
        {
            ResponseInfo response = new ResponseInfo();
            try
            {
                listKansei.AddRange(listKansei);
                List<List<double>> listType = new List<List<double>>();
                //Kansei's weight
                for (int i = 0; i < type.Count; i++)
                    listType[i] = Kansei.tinhTrongSo(listKansei.Where(p => p.Type == type[i]).Select(p => p.Point).ToList());

                //Point multiplied by weight
                kanseiPreprocess = pointMultiplyW(listKansei, listType);
                response.data = kanseiPreprocess;
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                response.statusCode = System.Net.HttpStatusCode.BadRequest;
                return await Task.FromResult(response);
            }
        }
        private static List<Kansei> pointMultiplyW(List<Kansei> listKanseiType, List<List<double>> w)
        {
            int index1 = 0;

            //listKanseiType.ForEach(item =>
            //{
            //    switch (item.Type)
            //    {
            //        case "TL01": item.Point *= w1[index1]; index1++; break;
            //        case "TL02": item.Point *= w2[index2]; index2++; break;
            //        case "TL03": item.Point *= w3[index3]; index3++; break;
            //    }
            //});

            for (int i = 0; i < type.Count; i++)
            {
                index1 = 0;
                listKanseiType.ForEach(item =>
                {
                    //switch (item.Type)
                    //{
                    //    case "TL01": item.Point *= w1[index1]; index1++; break;
                    //    case "TL02": item.Point *= w2[index2]; index2++; break;
                    //    case "TL03": item.Point *= w3[index3]; index3++; break;
                    //}
                    if (item.Type == type[i])
                    {
                        item.Point *= w[i][index1];
                        index1++;
                    }
                });
            }

            return listKanseiType;
        }
    }
}
