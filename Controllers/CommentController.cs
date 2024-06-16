using API_BLOG.Models;
using API_BLOG.Models.Dtos.Comment;
using API_BLOG.Models.Entitys;
using API_BLOG.Models.Specifications;
using API_BLOG.Repository;
using API_BLOG.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using System.Reflection.Metadata;

namespace API_BLOG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public CommentController(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetComment()
        {
            try
            {
                IEnumerable<Comment> commentLits = await _commentRepository.GetAll();
                _response.Resultado = _mapper.Map<IEnumerable<CommentDto>>(commentLits);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso= false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpGet("CommentPaginado")]
        [Authorize(Roles = "admin, user", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public  ActionResult<APIResponse> GetCommentPaginado([FromQuery] Parameters parameters, [FromQuery] CommentSearchDto comment)
        {
            try
            {
                var filter = _commentRepository.GetBy(comment);
                var commentList = _commentRepository.GetAllPaginado(parameters, filter: filter, includProperties:"Usuario");
                _response.Resultado = _mapper.Map<IEnumerable<CommentDto>>(commentList);
                _response.StatusCode = HttpStatusCode.OK;
                _response.TotalPaginas = commentList.MetaData.TotalPages;
                return Ok(_response);

            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages= new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpGet("{id:int}", Name ="GetComment")]
        public async Task<ActionResult<APIResponse>> GetComment(int id)
        {
            try
            {
                if(id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }

                var comment  = await _commentRepository.Get(c => c.Id == id, includProperties: "Usuario");
                if(comment is null)
                {
                    _response.StatusCode=HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<CommentDto>(comment);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin, user", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateComment([FromBody] CommentCreateDto comment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if(comment is null)
                {
                    return BadRequest(comment);
                }

                Comment modelo = _mapper.Map<Comment>(comment);

                modelo.CreateOn =  DateTime.Now;
                modelo.UpdateOn = DateTime.Now;
                await _commentRepository.Create(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetComment", new { Id = modelo.Id }, _response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }

            return _response;
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "admin, user", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateComment(int id, [FromBody]CommentUpdateDto commentUpdate)
        {
            var oldComment = await _commentRepository.Get(c => c.Id == id);

            if (oldComment is null)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return BadRequest(_response);
            }

            if(commentUpdate is null)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Comment modelo = _mapper.Map<Comment>(commentUpdate);
            modelo.Id = id;
            modelo.CreateOn = oldComment.CreateOn;

            await _commentRepository.Update(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin, user", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var comment = await _commentRepository.Get(x => x.Id == id);

                if (comment is null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }

                await _commentRepository.Delete(comment);
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
