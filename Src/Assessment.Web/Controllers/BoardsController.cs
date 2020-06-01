using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assessment.Web.Models;
using Newtonsoft.Json;

namespace Assessment.Web.Controllers
{
    [Route("api/[controller]")]
    public class BoardsController : Controller
    {
        public IBoardRepository boards;

        public BoardsController(IBoardRepository boards)
        {
            this.boards = boards;
        }

        [HttpGet]
        public IEnumerable<Board> GetAll()
        {
            return boards.GetAll();
        }

        [HttpGet("{id}")]
        public Board Find(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Board ID must be greater than zero.");

            return boards.Find(id);
        }
        [HttpPost]
        public void Post([FromBody] Board board)
        {
            if (board.Id <= 0) throw new ArgumentOutOfRangeException(nameof(board.Id), "Board ID must be greater than zero.");
            if (string.IsNullOrEmpty(board.Name)) throw new ArgumentNullException(nameof(board.Name));
            if (!boards.Add(board))
            {
                throw new Exception("Unable to Add new board");
            }

        }
        [HttpDelete("{boardId}")]
        public void Delete(int boardId)
        {
            if (boardId <= 0) throw new ArgumentOutOfRangeException(nameof(boardId), "Board ID must be greater than zero.");
            if(!boards.Delete(boardId))
            {
                throw new Exception("Unable to delete board because it might not exist or for other reasons");

            }
        }

        [HttpPost("{board-id}/post-its")]
        public void PostPins([FromRoute(Name = "board-id")] int boardId, [FromBody] PostIt postit)
        {
            if (boardId <= 0) throw new ArgumentOutOfRangeException(nameof(boardId), "Board ID must be greater than zero.");
            if(!boards.AddPostIt(boardId, postit))
            {
                throw new Exception("Unable to Add Post Its to board");

            }
        }
    }
}
