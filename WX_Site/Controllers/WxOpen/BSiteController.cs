using _2BSite.Service.DTO.WX;
using _2BSite.Service.Interface.WX;
using _2BSite.Service.Model;
using Core.Database.Repository;
using Core.Infrastructure;
using Core.Redis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Senparc.Weixin.WxOpen.Containers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WX_Site.Common;
using WX_Site.Model;
using WXSite.Database;
using WXSite.Database.Entities;
using HttpRequest = WX_Site.Common.HttpRequest;

namespace WX_Site.Controllers.WxOpen
{
    [ApiController]
    [Route("[controller]")]
    public class BSiteController : Controller
    {
        private Core.Redis.ICacheClient _cacheClient;
        private IServiceProvider _serviceProvider;
        private IUnitOfWork<WXContext> _unitOfWork;
        private IQuestionMenuService _questionMenuService;
        private IQuestionsService _questionsService;
        private IHistoryService _historyService;
        private IErrorService _errorService;
        private IFeedBackService _feedBackService;
        private IUserService _userService;
        private IConfiguration _configuration;
        private IHostingEnvironment _hostingEnvironment;

        public BSiteController(ICacheClient cacheClient, IServiceProvider serviceProvider, IUnitOfWork<WXContext> unitOfWork)
        {
            _serviceProvider = serviceProvider;
            _cacheClient = cacheClient;
            _unitOfWork = unitOfWork;

            _questionMenuService = serviceProvider.GetService(typeof(IQuestionMenuService)) as IQuestionMenuService;
            _questionsService = serviceProvider.GetService(typeof(IQuestionsService)) as IQuestionsService;
            _historyService = serviceProvider.GetService(typeof(IHistoryService)) as IHistoryService;
            _errorService = serviceProvider.GetService(typeof(IErrorService)) as IErrorService;
            _feedBackService = serviceProvider.GetService(typeof(IFeedBackService)) as IFeedBackService;
            _userService = serviceProvider.GetService(typeof(IUserService)) as IUserService;
            _configuration = serviceProvider.GetService(typeof(IConfiguration)) as IConfiguration;
            _hostingEnvironment = serviceProvider.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
            //获取子连接
            var connect = _unitOfWork.DbContext.Database.GetDbConnection();

            if (Core.Infrastructure.Global.DBRWManager.IsMaterConnection(typeof(WXContext).ToString(), connect.ConnectionString))
            {
                if (connect.State == System.Data.ConnectionState.Closed)
                {
                    _unitOfWork.DbContext.Database.GetDbConnection().ConnectionString = Global.DBRWManager.GetSlave(typeof(WXContext).ToString());
                }
            }
        }
        /// <summary>
        /// 获取套题列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetQuestionMenuList")]
        public IActionResult GetQuestionMenuList(string sessionId)
        {
            var sessionBag = SessionContainer.GetSession(sessionId);
            if (sessionBag == null)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "用户未正确登录!" });
            }
            var questionMenus = _questionMenuService.GetAll().ToList();
            return Json(questionMenus);
        }
        /// <summary>
        /// 获取套题详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetQuestionMenuById")]
        public IActionResult GetQuestionMenuById(int Id, string sessionId)
        {
            var sessionBag = SessionContainer.GetSession(sessionId);
            if (sessionBag == null)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "用户未正确登录!" });
            }
            var questionMenus = _questionMenuService.GetAll().Where(t => t.Id == Id).FirstOrDefault();
            return Json(questionMenus);
        }
        /// <summary>
        /// 获取对应题库
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetQuestionsByMenu")]
        public IActionResult GetQuestionsByMenu(int Id, string sessionId)
        {
            var sessionBag = SessionContainer.GetSession(sessionId);
            if (sessionBag == null)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "用户未正确登录!" });
            }
            var questions = _questionsService.GetAll().Where(t => t.MenuId == Id && t.Type == "1").ToList();
            var singles = Common.Common.getRandomList(questions, 40);
            questions = _questionsService.GetAll().Where(t => t.MenuId == Id && t.Type == "2").ToList();
            var multiple = Common.Common.getRandomList(questions, 40);
            questions = _questionsService.GetAll().Where(t => t.MenuId == Id && t.Type == "3").ToList();
            var judge = Common.Common.getRandomList(questions, 20);
            singles.AddRange(multiple);
            singles.AddRange(judge);
            foreach (var item in singles)
            {
                item.ChoseListObj = JArray.Parse(item.ChoseList);
            }
            return Json(singles);
        }
        /// <summary>
        /// 保存答题历史记录
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveHistory")]
        public IActionResult SaveHistory(HistoryDTO historyDTO)
        {
            var sessionBag = SessionContainer.GetSession(historyDTO.sessionId);
            if (sessionBag == null)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "用户未正确登录!" });
            }
            historyDTO.CreateTime = DateTime.Now;
            historyDTO.Creator = "admin1";
            historyDTO.QuestionList = JsonConvert.SerializeObject(historyDTO.QuestionListObj);
            var res = _historyService.Add(historyDTO);
            if (res.Code != 0)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "发生未知错误，请稍后再试！" });
            }
            return Json(new ReturnResultModel() { Success = true, Message = "成功!", Data = ((History)res.Result).Id });
        }
        /// <summary>
        /// 获取个人答题历史记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHistoryList")]
        public IActionResult GetHistoryList(int UserId, string sessionId)
        {
            var sessionBag = SessionContainer.GetSession(sessionId);
            if (sessionBag == null)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "用户未正确登录!" });
            }
            var data = _historyService.GetAll().Where(t => t.UserId == UserId).Select(t=>new { t.UserId,t.QuestionMenu,t.Id,t.CreateTime,t.Creator,t.MenuId}).ToList();
            return Json(data);
        }
        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetHistory")]
        public IActionResult GetHistory(int Id, string sessionId)
        {
            var sessionBag = SessionContainer.GetSession(sessionId);
            if (sessionBag == null)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "用户未正确登录!" });
            }
            var data = _historyService.GetAll().Where(t => t.Id == Id).FirstOrDefault();
            var average = _historyService.GetAll().Where(t => t.MenuId == data.MenuId).GroupBy(t => t.MenuId).Select(t => new { score = t.Sum(o => o.Score), num = t.Count() }).FirstOrDefault();
            if (average != null)
            {
                data.AverageScore = average.score / average.num;
            }
            else
            {
                data.AverageScore = 0;
            }
            var beatNum = _historyService.GetAll().Where(t => t.Score < data.Score).Count();
            data.BeatNum = beatNum;
            data.QuestionListObj = JArray.Parse(data.QuestionList);
            return Json(data);
        }
        /// <summary>
        /// 保存错题记录
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveError")]
        public IActionResult SaveError(ErrorDTO errorDTO)
        {
            var sessionBag = SessionContainer.GetSession(errorDTO.sessionId);
            if (sessionBag == null)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "用户未正确登录!" });
            }
            errorDTO.CreateTime = DateTime.Now;
            errorDTO.Creator = "admin1";
            errorDTO.QuestionList = JsonConvert.SerializeObject(errorDTO.QuestionListObj);
            var res = _errorService.Add(errorDTO);
            if (res.Code != 0)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "发生未知错误，请稍后再试！" });
            }
            return Json(new ReturnResultModel() { Success = true, Message = "成功!" });
        }
        /// <summary>
        /// 获取错题记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetErrorList")]
        public IActionResult GetErrorList(int MenuId,int UserId, string sessionId)
        {
            var sessionBag = SessionContainer.GetSession(sessionId);
            if (sessionBag == null)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "用户未正确登录!" });
            }
            var data = _errorService.GetAll().Where(t => t.MenuId == MenuId && t.UserId == UserId).OrderByDescending(t=>t.CreateTime).FirstOrDefault();
            if (data!=null)
            {
                data.QuestionListObj = JArray.Parse(data.QuestionList);
                data.QuestionList = "";
                return Json(data);
            }
            return Json(data);
        }
        /// <summary>
        /// 保存反馈意见
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveFeedBack")]
        public IActionResult SaveFeedBack(FeedBackDTO feedBackDTO)
        {
            feedBackDTO.CreateTime = DateTime.Now;
            feedBackDTO.Creator = "admin1";
            var res = _feedBackService.Add(feedBackDTO);
            if (res.Code != 0)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "发生未知错误，请稍后再试！" });
            }
            return Json(new ReturnResultModel() { Success = true, Message = "成功!" });
        }
        /// <summary>
        /// 获取排名信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRank")]
        public IActionResult GetRank(int MenuId, string sessionId)
        {
            var sessionBag = SessionContainer.GetSession(sessionId);
            if (sessionBag == null)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "用户未正确登录!" });
            }
            var data = _historyService.GetAll().Where(t => t.MenuId == MenuId).OrderByDescending(t => t.Score).Select(t=>new HistoryDTO { UserId= t.UserId, Score=t.Score, AvatarUrl ="",NickName=""}).ToList();
            var users = _userService.GetAll().Where(t => data.Select(o => o.UserId).Contains(t.Id)).ToList();
            data.ForEach(t => {
                t.NickName = users.Where(o => o.Id == t.UserId).Select(t => t.NickName).FirstOrDefault();
                t.AvatarUrl = users.Where(o => o.Id == t.UserId).Select(t => t.AvatarUrl).FirstOrDefault();
            });
            return Json(data);
        }
        /// <summary>
        /// 保存用户基本信息
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPost("SaveUserInfo")]
        public IActionResult SaveUserInfo(UserDTO userDTO)
        {
            userDTO.CreateTime = DateTime.Now;
            userDTO.Creator = "admin";
            var exist = _userService.GetAll().Where(t => t.AuthData == userDTO.AuthData).FirstOrDefault();
            DBResult data;
            if (exist != null)
            {
                exist.AvatarUrl = userDTO.AvatarUrl;
                exist.NickName = userDTO.NickName;
                exist.Password = userDTO.Password;
                exist.UserName = userDTO.UserName;
                exist.Modifier = "admin1";
                exist.ModifyTime = DateTime.Now;
                data = _userService.Update(exist);
            }
            else
            {
                data = _userService.Add(userDTO);
            }
            
            if (data.Code != 0)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "发生未知错误，请稍后再试！" });
            }
            return Json(new ReturnResultModel() { Success = true, Message = "成功!" });
        }
        /// <summary>
        /// 保存电话号码
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet("SavePhoneNumber")]
        public IActionResult SavePhoneNumber(string openId,string phone)
        {
            var data = _userService.GetAll().Where(t => t.AuthData == openId).FirstOrDefault();
            if (data != null)
            {
                data.MobilePhoneNumber = phone;
                var res = _userService.Update(data);
                if (res.Code != 0)
                {
                    return Json(new ReturnResultModel() { Success = false, Message = "发生未知错误，请稍后再试！" });
                }
                else
                {
                    return Json(new ReturnResultModel() { Success = true, Message = "成功!" });
                }
            }
            else
            {
                return Json(new ReturnResultModel() { Success = false, Message = "用户不存在，请稍后再试！" });
            }
        }
        /// <summary>
        /// 人脸检测
        /// </summary>
        /// <returns></returns>
        [HttpPost("FaceTest")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult FaceTest()
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count == 0)
            {
                return Json(new ReturnResultModel() { Success = false, Message = "无上传文件" });
            }
            List<string> _extns = new List<string>() { ".jpg", ".png", ".bmp", ".ico", ".heif", ".jpeg", ".heic"};
            var extension = string.Empty;
            FaceModel faceModel=null;
            foreach (var upload in files)
            {
                extension = Path.GetExtension(upload.FileName).ToLower();
                if (_extns.Select(x => x.ToLower()).ToList().Contains(extension) == false)
                {
                    return Json(new ReturnResultModel() { Success = false, Message = "上传文件类型不对" });
                }
                Stream streams = upload.OpenReadStream();
                byte[] bytes = new byte[streams.Length];
                streams.Read(bytes, 0, bytes.Length);

                

                MemoryStream ms = new MemoryStream(bytes);
                Image img = null;
                img = Image.FromStream(ms);
                Random ran = new Random();
                int random = ran.Next(10000, 99999);
                var newFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + random + extension;
                var phyconfigPath = _configuration.GetValue<string>("UploadImagePath") + "\\WXSite";
                var physicalPath = phyconfigPath + "\\" + newFileName;
                if (!Directory.Exists(phyconfigPath)) //如果该文件夹不存在就建立这个新文件夹
                {
                    Directory.CreateDirectory(phyconfigPath);
                }
                img.Save(physicalPath, ImageFormat.Jpeg);
                img.Dispose();
                img = null;
                ms.Dispose();
                streams.Dispose();

                #region 请求接口
                var client = new RestClient("http://www.yanzhiceshi.com/index.php/Test/index.html");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Cookie", "PHPSESSID=pk520iqo9bfhdufj52ems7fk4g");
                request.AddFile("img", _hostingEnvironment.ContentRootPath + physicalPath.Substring(1));
                //request.AddFile("img", "C:/Users/FS/source/2BSite/2BSite/WX_Site/wwwroot/upload/images/WXSite/2021061713143721202.jpg");
                //request.AddFile("img", "C:/Users/FS/Pictures/Camera Roll/IMG_0802.JPG");
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                faceModel = JsonConvert.DeserializeObject<FaceModel>(response.Content);
                #endregion
            }

            return Json(new ReturnResultModel() { Success = true, Message = "成功",Data=faceModel });
        }

        [HttpPost("UploadFile")]
        public string UploadFile()
        {
            if (Request.Form.Files.Count == 0)
                return "未检测到文件";
            string path = _hostingEnvironment.ContentRootPath + "\\wwwroot\\Files";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            var file = Request.Form.Files[0];
            string fileExt = file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            string filename = Guid.NewGuid().ToString() + "." + fileExt;
            string fileFullName = path + "\\" + filename;
            using (FileStream fs = System.IO.File.Create(fileFullName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            return "/Files/" + filename;
        }
    }
}
