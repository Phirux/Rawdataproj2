﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataRepository;
using DataRepository.Dto.QuestionDto;
using DomainModel;
using Microsoft.AspNetCore.Mvc;
using WebService.Models.Question;

namespace WebService.Controllers
{
    [Route("api/questions")]
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _QuestionRepository;
        private readonly IMapper _Mapper;

        public QuestionController(IQuestionRepository QuestionRepository, IMapper Mapper)
        {
            _QuestionRepository = QuestionRepository;
            _Mapper = Mapper;
        }

        [HttpGet("{id}", Name = nameof(GetQuestion))]
<<<<<<< HEAD
        public async Task<ActionResult> GetQuestion(int id)
        { 

            var question =  await _QuestionRepository.GetQuestion(id); 
            if (question == null) return NotFound(); 

            var model = _Mapper.Map<QuestionModel>(question);
             
            return  Ok(model);
        }

        //[HttpGet("{id}", Name = nameof(GetQuestion))]
        //public async Task<ActionResult> GetQuestion(int id)
        //{
        //    var question = _QuestionRepository.Get(id);
        //    if (question == null) return NotFound();
=======
        public async Task<IActionResult> GetQuestion(int id)
        {
            var question = await _QuestionRepository.Get(id);
            if (question == null) return NotFound();

            QuestionModel model = new QuestionModel
            {
                Url = CreateLink(question.ID)
            };

            model = _Mapper.Map<QuestionModel>(question);
>>>>>>> c1dd4c8515d64ec8b881ce3d9f0e804f80fef1ab

        //    QuestionModel model = new QuestionModel
        //    {
        //        Url = CreateLink(question.Id)
        //    };

        //    model = _Mapper.Map<QuestionModel>(question.Result);


        //    return Ok(model);
        //}

        [HttpGet(Name = nameof(GetQuestions))]
        public async Task<IActionResult> GetQuestions(PagingInfo pagingInfo)
        {
            var question = await _QuestionRepository.GetAll(pagingInfo);
            IEnumerable<QuestionListModel> model = question.Select(que => CreateQuestionListModel(que));

            var total = _QuestionRepository.Count();
            var pages = (int)Math.Ceiling(total / (double)pagingInfo.PageSize);

            var prev = pagingInfo.Page > 0
                ? Url.Link(nameof(GetQuestions),
                    new { page = pagingInfo.Page - 1, pagingInfo.PageSize })
                : null;

            var next = pagingInfo.Page < pages - 1
                ? Url.Link(nameof(GetQuestions),
                    new { page = pagingInfo.Page + 1, pagingInfo.PageSize })
                : null;

            var result = new
            {
                Prev = prev,
                Next = next,
                Total = total,
                Pages = pages,
                Questions = model
            }; 

            return Ok(result);
        }


        [HttpGet("{id}/answers", Name = nameof(GetQuestionAnswers))]
        public async Task<IActionResult> GetQuestionAnswers(int id)
        {
            var queAnswers = await _QuestionRepository.GetQuestionAnswers(id);
            if (queAnswers == null) return NotFound();

            var model = queAnswers.Select(que => CreateQuestionAnswersModel(que)); 

            return Ok(model);
        }


        [HttpGet("{id}/comments", Name = nameof(GetQuestionComments))]
        public async Task<IActionResult> GetQuestionComments(int id)
        {
            var queCom = await _QuestionRepository.GetQuestionComments(id);
            if (queCom == null) return NotFound();

            var model = queCom.Select(que => CreateQuestionCommnetsModel(que));

            return Ok(model);
        }

<<<<<<< HEAD
        //private object GetAnswer()
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<ActionResult> GetAnswer(int id)
        //{
        //    var answer = await _QuestionRepository.GetAnswer(id);
        //    if (answer == null) return NotFound();

        //    var model = answer.Select(que => CreateQuestionAnswersModel(que));
=======
        //[HttpPost]
        //public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionModel model)
        //{
        //    if (model == null) return BadRequest();

        //    var newQuestion = await _QuestionRepository.Add(model);

        //    return Ok(newQuestion);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Put(int id, [FromBody]string value)
        //{
          

        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteQuestion(int id)
        //{
            
        //}

>>>>>>> c1dd4c8515d64ec8b881ce3d9f0e804f80fef1ab

        //    return Ok(model);
        //}

        /*******************************************************
         * Helpers
         * *****************************************************/
        private QuestionCommentsModel CreateQuestionCommnetsModel(QuestionCommentsDto questionCom)
        {
            var model = new QuestionCommentsModel
            {
                ID = questionCom.ID,
                Score = questionCom.Score,
                Creationdate = questionCom.Creationdate,
                Test = questionCom.Text
            }; 
            return model;
        }

        private QuestionAnswersModel CreateQuestionAnswersModel(QuestionAnswersDto question)
        {
            var model = new QuestionAnswersModel
            {
                 Body = question.Body,
                 Creationdate = question.Creationdate,
                 Score = question.Score
            }; 
            return model;
        }

        private QuestionListModel CreateQuestionListModel(Question question)
        {
            var model = new QuestionListModel
            {
                 Title = question.Title, 
            };
            model.Url = CreateQuestionLink(question.ID);
            return model;
        }
          
        private string CreateQuestionLink(int id)
        {
            return Url.Link(nameof(GetQuestion), new { id });
        }

        //private string CreateAnswerLink(int id)
        //{
        //    return Url.Link(nameof(GetAnswer), new { id });
        //}

      
    }
}