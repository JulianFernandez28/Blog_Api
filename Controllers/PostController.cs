﻿using API_BLOG.Models;
using API_BLOG.Models.Dtos.Post;
using API_BLOG.Models.Entitys;
using API_BLOG.Models.Specifications;
using API_BLOG.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API_BLOG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public PostController(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPosts()
        {
            try
            {
                IEnumerable<Post> postList = await _postRepository.GetAll();
                _response.Resultado = _mapper.Map<IEnumerable<PostDto>>(postList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("PostPaginado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<APIResponse> GetPostPaginado([FromQuery]Parameters parameters)
        {
            try
            {
                var postList = _postRepository.GetAllPaginado(parameters, filter: (p=> p.Status == true), includProperties: "Usuario") ;
                _response.Resultado = _mapper.Map<IEnumerable<PostDto>>(postList);
                _response.StatusCode = HttpStatusCode.OK;
                _response.TotalPaginas = postList.MetaData.TotalPages;
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsExitoso = true;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpGet("{id:int}", Name ="GetPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPost(int id)
        {
            try
            {
                if(id == 0)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso=false;
                    return BadRequest(_response);
                }

                var post = await _postRepository.Get(v=> v.Id == id, includProperties:"Usuario");
                if (post is null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Resultado= _mapper.Map<PostDto>(post);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreatePost([FromBody]PostCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _postRepository.Get(x=> x.Title.ToLower() == createDto.Title.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "El post con ese titulo ya existe");

                    return BadRequest(ModelState);
                }

                if(createDto is null)
                {
                    return BadRequest(createDto);
                }

                Post modelo =  _mapper.Map<Post>(createDto);

                modelo.CreateOn = DateTime.Now;
                modelo.UpdateOn = DateTime.Now;
                await _postRepository.Create(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetPost", new { Id = modelo.Id }, _response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePost(int id, [FromBody]PostUpdateDto updateDto) 
        {
            if(updateDto == null)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Post modelo = _mapper.Map<Post>(updateDto);

            await _postRepository.Update(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if(id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var post = await _postRepository.Get(x => x.Id == id);

                if(post is null)
                {
                    _response.IsExitoso=false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }

                await _postRepository.Delete(post);
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return Ok(_response);
        }
    }
}
