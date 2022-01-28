using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyProfileController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;
        public MyProfileController(StoreContext context, IMapper mapper, ImageService imageService)
        {
            _imageService = imageService;
            _mapper = mapper;
            _context = context;
        }

        // [HttpGet]
        // public async Task<ActionResult<PagedList<Product>>> GetProfile([FromQuery] ProductParams productParams)
        // {
        //     var query = _context.Products
        //         .Sort(productParams.OrderBy)
        //         .Search(productParams.SearchTerm)
        //         .Filter(productParams.Brands, productParams.Types)
        //         .AsQueryable();

        //     var products = await PagedList<Product>.ToPagedList(query,
        //         productParams.PageNumber, productParams.PageSize);

        //     Response.AddPaginationHeader(products.MetaData);

        //     return products;
        // }

        [HttpGet("{id}", Name = "GetProfile")]
        public async Task<ActionResult<MyProfile>> GetProfile(int id)
        {
            var myprofile = await _context.MyProfiles.FindAsync(id);

            if (myprofile == null) return NotFound();

            return myprofile;
        }

        // [HttpGet("filters")]
        // public async Task<IActionResult> GetFilters()
        // {
        //     var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
        //     var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();

        //     return Ok(new { brands, types });
        // }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MyProfile>> CreateProfile([FromForm] CreateProfileDto profileDto)
        {
            var mprofile = _mapper.Map<MyProfile>(profileDto);

            if (profileDto.File != null)
            {
                var imageResult = await _imageService.AddImageAsync(profileDto.File);

                if (imageResult.Error != null)
                    return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });

                mprofile.PictureUrl = imageResult.SecureUrl.ToString();
                mprofile.PublicId = imageResult.PublicId;
            }

            _context.MyProfiles.Add(mprofile);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetProfile", new { Id = mprofile.Id }, mprofile);

            return BadRequest(new ProblemDetails { Title = "Problem completing your profile" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct([FromForm] UpdateProfileDto profileDto)
        {
            var mproduct = await _context.MyProfiles.FindAsync(profileDto.Id);

            if (mproduct == null) return NotFound();

            _mapper.Map(profileDto, mproduct);

            if (profileDto.File != null)
            {
                var imageResult = await _imageService.AddImageAsync(profileDto.File);

                if (imageResult.Error != null)
                    return BadRequest(new ProblemDetails { Title = imageResult.Error.Message });

                if (!string.IsNullOrEmpty(mproduct.PublicId))
                    await _imageService.DeleteImageAsync(mproduct.PublicId);

                mproduct.PictureUrl = imageResult.SecureUrl.ToString();
                mproduct.PublicId = imageResult.PublicId;
            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(mproduct);

            return BadRequest(new ProblemDetails { Title = "Problem updating the profile" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var mproduct = await _context.MyProfiles.FindAsync(id);

            if (mproduct == null) return NotFound();

            if (!string.IsNullOrEmpty(mproduct.PublicId))
                await _imageService.DeleteImageAsync(mproduct.PublicId);

            _context.MyProfiles.Remove(mproduct);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting the profile" });
        }
    }
}