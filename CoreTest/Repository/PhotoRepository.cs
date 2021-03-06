﻿using CoreTest.Models;
using CoreTest.Models.Entity;
using CoreTest.Repository.Interfaces;

namespace CoreTest.Repository
{
    public class PhotoRepository : BaseRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(PhotoContext context) : base(context)
        {
        }
    }
}
