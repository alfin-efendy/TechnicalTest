using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalTest.Models;

namespace TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="context">The application context.</param>
        public UserController(ApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the action result that wraps the collection of users.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            // Return all users
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the action result that wraps the user.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            // Find the user by ID
            var user = await _context.Users.FindAsync(id);
            
            // If the user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound();
            }

            // Return the user
            return user;
        }

        /// <summary>
        /// Updates a specific user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="user">The updated user object.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the action result.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            
            try
            {
                // Save the changes
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if the user exists
                bool userExists = _context.Users.Any(e => e.Id == id);

                // If the user does not exist, return a 404 Not Found response
                if (!userExists)
                {
                    return NotFound();
                }
                else
                {
                    // Otherwise, re-throw the exception
                    throw;
                }
            }

            // Return a 204 No Content response
            return NoContent();
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user object to create.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the action result that wraps the created user.</returns>
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            // Add the user to the context
            _context.Users.Add(user);

            // Save the changes
            await _context.SaveChangesAsync();


            // Return a 201 Created response
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        /// <summary>
        /// Deletes a specific user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the action result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            // Find the user by ID
            var user = await _context.Users.FindAsync(id);

            // If the user is not found, return a 404 Not Found response
            if (user == null)
            {
                return NotFound();
            }

            // Remove the user from the context
            _context.Users.Remove(user);

            // Save the changes
            await _context.SaveChangesAsync();

            // Return a 204 No Content response
            return NoContent();
        }
    }
}
