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

        [HttpPost("",Name = "PostNewKansei")]
        public async Task<ActionResult<ResponseInfo>> addKansei(List<Kansei> listKansei)
        {
            ResponseInfo response = new ResponseInfo();
            try
            {
                listKansei.AddRange(listKansei);
                //Kansei's weight
                List<double> listType001 = Kansei.tinhTrongSo(listKansei.Where(p => p.Type == "TL01").Select(p => p.Point).ToList());
                List<double> listType002 = Kansei.tinhTrongSo(listKansei.Where(p => p.Type == "TL02").Select(p => p.Point).ToList());
                List<double> listType003 = Kansei.tinhTrongSo(listKansei.Where(p => p.Type == "TL03").Select(p => p.Point).ToList());

                //Point multiplied by weight
                kanseiPreprocess = pointMultiplyW(listKansei, listType001, listType002, listType003);
                response.data = kanseiPreprocess;
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                response.statusCode = System.Net.HttpStatusCode.BadRequest;
                return await Task.FromResult(response);
            }
        }
        private static List<Kansei> pointMultiplyW(List<Kansei> listKanseiType, List<double> w1, List<double> w2, List<double> w3)
        {
            int index1 = 0;
            int index2 = 0;
            int index3 = 0;
            listKanseiType.ForEach(item =>
            {
                switch (item.Type)
                {
                    case "TL01": item.Point *= w1[index1]; index1++; break;
                    case "TL02": item.Point *= w2[index2]; index2++; break;
                    case "TL03": item.Point *= w3[index3]; index3++; break;
                }
            });

            return listKanseiType;
        }
    }
}
