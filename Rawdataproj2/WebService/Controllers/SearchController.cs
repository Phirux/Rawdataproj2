﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataRepository; 
using DataRepository.Dto.SearchDto;
using DomainModel;
using Microsoft.AspNetCore.Mvc;
using WebService.Models;
using WebService.Models.Search;

namespace WebService.Controllers
{  
    [Route("api/search")]
    public class SearchController : Controller
    {
        private readonly ISearchRepository _SearchRepository;
        private readonly IMapper _Mapper;

        public SearchController(ISearchRepository SearchRepository, IMapper Mapper)
        {
            _SearchRepository = SearchRepository;
            _Mapper = Mapper;
        }

        [HttpGet(Name = nameof(Search))]
        public async Task<IActionResult> Search(string query, PagingInfo pagingInfo, string method = null, string sortby = null, string orderby = null)
        {
            if(string.IsNullOrEmpty(query))
            {
                return Ok("No query provided");
            }
            
            var search = await UseFetchingMethod(query, pagingInfo, method, sortby, orderby);

            if (search == null && search.Count <= 0) return NotFound();

            var model = search.Select(s => CreateSearchResultModel(s));

            var urls = GetUrls(query, pagingInfo, method, sortby, orderby);
            var prev = urls[0];
            var next = urls[1];
            var total = search.Count();
            const string returnType = "posts";
            var result = PagingHelper.GetPagingResult(pagingInfo, total, model, returnType, prev, next);
          
            return Ok(result);
        }


        /*******************************************************
         * Helpers
         * *****************************************************/

        private async Task<IList<SearchResultDto>> UseFetchingMethod(string query, PagingInfo pagingInfo, string method, string sortby, string orderby)
        {
            IList<SearchResultDto> search = new List<SearchResultDto>();
            switch (method)
            {
                case "\"\"":
                    var option1 = await _SearchRepository.Bestmatch(query, pagingInfo, method, sortby, orderby);
                    option1.ToList().ForEach(s => { search.Add(s); });
                    break;
                case "\"bestmatch\"":
                    var option2 = await _SearchRepository.Bestmatch(query, pagingInfo, method, sortby, orderby);
                    option2.ToList().ForEach(s => { search.Add(s); });
                    break;
                case "\"matchall\"":
                    var option3 = await _SearchRepository.MatchAll(query, pagingInfo, method, sortby, orderby);
                    option3.ToList().ForEach(s => { search.Add(s); });
                    break;
                default:
                    var defaultOption = await _SearchRepository.Bestmatch(query, pagingInfo, method, sortby, orderby);
                    defaultOption.ToList().ForEach(s => { search.Add(s); });
                    break;
            }
            return search;
        }

        private string[] GetUrls(string query, PagingInfo pagingInfo, string method, string sortby, string orderby)
        {
            var prev = Url.Link(nameof(Search), new
            {
                query,
                method = string.IsNullOrEmpty(method) ? "" : method,
                sortby = string.IsNullOrEmpty(sortby) ? "" : sortby,
                orderby = string.IsNullOrEmpty(orderby) ? "" : orderby,
                page = pagingInfo.Page - 1, pagingInfo.PageSize
            });

            var next = Url.Link(nameof(Search), new
            {
                query,
                method = string.IsNullOrEmpty(method) ? "" : method,
                sortby = string.IsNullOrEmpty(sortby) ? "" : sortby,
                orderby = string.IsNullOrEmpty(orderby) ? "" : orderby,
                page = pagingInfo.Page + 1, pagingInfo.PageSize
            });

            var urls = new string[] { prev, next };

            return urls;
        }

        private SearchResultModel CreateSearchResultModel(SearchResultDto search)
        {
            var model = new SearchResultModel
            {
                Id = search.Id,
                Rank = search.Rank,
                Body = search.Body
            };
            return model;
        }

        private BestmatchModel CreateBestmatchModel(BestmatchDto search)
        {
            var model = new BestmatchModel
            {
                Id = search.Id,
                Rank = search.Rank
            };
            return model;
        }


        private MatchallModel CreateMatchAllhModel(MatchallDto search)
        {
            var model = new MatchallModel
            {
                Id = search.Id,
                Rank = search.Rank,
                Body = search.Body
            };
            return model;
        }

        private BestmatchRankedModel CreateBestMatchRankedModel(BestMatchRankedDto search)
        {
            var model = new BestmatchRankedModel
            {
                Id = search.Id,
                Rank = search.Rank 
            };
            return model;
        }

        private BestMatchWeightedModel CreateBestMatchWeightedModel(BestMatchWeightedDto search)
        {
            var model = new BestMatchWeightedModel
            {
                Id = search.Id,
                Rank = search.Rank 
            };
            return model;
        }

    }
}