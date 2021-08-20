using System;
using System.Collections.Generic;
using System.Linq;
using TheXPressGames.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumDemo.Controllers
{
    public class PostsController : Controller
    {
        private TheXPressContext db;

        private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }

        private bool isLoggedIn
        {
            get
            {
                return uid != null;
            }
        }

        public PostsController(TheXPressContext context)
        {
            db = context;
        }

        // 1. handles GET request to DISPLAY the form used to create a new Post
        [HttpGet("/posts/new")]
        public IActionResult New()
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            return View("New");
        }

        // 2. handles POST request form submission to CREATE a new Post model instance
        [HttpPost("/posts/create")]
        public IActionResult Create(Post newPost)
        {
            // Every time a form is submitted, check the validations.
            if (ModelState.IsValid == false)
            {
                // Go back to the form so error messages are displayed.
                return View("New");
            }

            newPost.UserId = (int)uid; // Relate the author to the post.

            // The above return did not happen so ModelState IS valid.
            db.Posts.Add(newPost);
            // db doesn't update until we run save changes
            // after SaveChanges, our newPost object now has it's PostId updated from db auto generated id
            db.SaveChanges();
            return RedirectToAction("All");
        }

        [HttpGet("/posts")]
        public IActionResult All()
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            List<Post> allPosts = db.Posts
                // Select what navigation properties from a Post you want to be included (JOIN).
                .Include(post => post.Author) // hover over the param to see it's data type
                .Include(post => post.Likes)
                .ToList();
            return View("Homepage", allPosts);

            /* 
            The db.Posts and the .Include did this:
            SELECT * FROM posts AS p
            JOIN users AS u ON u.UserId = p.UserId
            */
        }

        [HttpGet("/posts/{postId}")]
        public IActionResult Details(int postId)
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            Post post = db.Posts
                .Include(post => post.Author)
                .Include(post => post.Likes)
                // Include something from the last thing that was included.
                // Include the User from the likes (hover over like param to see data type)
                .ThenInclude(like => like.User)
                .FirstOrDefault(p => p.PostId == postId);

            if (post == null)
            {
                return RedirectToAction("All");
            }

            return View("Details", post);
        }

        [HttpPost("/posts/{postId}/delete")]
        public IActionResult Delete(int postId)
        {
            Post post = db.Posts.FirstOrDefault(p => p.PostId == postId);

            if (post == null)
            {
                return RedirectToAction("All");
            }

            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("All");
        }

        [HttpGet("/posts/{postId}/edit")]
        public IActionResult Edit(int postId)
        {
            Post post = db.Posts.FirstOrDefault(p => p.PostId == postId);

            // The edit button will be hidden if you are not the author,
            // but the user could still type the URL in manually, so
            // prevent them from editing if they are not the author.
            if (post == null || post.UserId != uid)
            {
                return RedirectToAction("All");
            }

            return View("Edit", post);
        }

        [HttpPost("/posts/{postId}/update")]
        public IActionResult Update(int postId, Post editedPost)
        {
            if (ModelState.IsValid == false)
            {
                editedPost.PostId = postId;
                // Send back to the page with the current form edited data to
                // display errors.
                return View("Edit", editedPost);
            }

            Post dbPost = db.Posts.FirstOrDefault(p => p.PostId == postId);

            if (dbPost == null)
            {
                return RedirectToAction("All");
            }

            dbPost.Topic = editedPost.Topic;
            dbPost.Body = editedPost.Body;
            dbPost.ImgUrl = editedPost.ImgUrl;
            dbPost.UpdatedAt = DateTime.Now;

            db.Posts.Update(dbPost);
            db.SaveChanges();

            /* 
            When redirecting to action that has params, you need to pass in a
            dict with keys that match param names and the value of the keys are
            the values for the params.
            */
            return RedirectToAction("Details", new { postId = postId });
        }

        [HttpPost("/posts/{postId}/like")]
        public IActionResult Like(int postId)
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            UserPostLike existingLike = db.UserPostLikes
                .FirstOrDefault(like => like.UserId == (int)uid && like.PostId == postId);

            if (existingLike == null)
            {
                UserPostLike like = new UserPostLike()
                {
                    PostId = postId,
                    UserId = (int)uid
                };

                db.UserPostLikes.Add(like);
            }
            else
            {
                db.UserPostLikes.Remove(existingLike);
            }


            db.SaveChanges();
            return RedirectToAction("All");
        }
    }
}