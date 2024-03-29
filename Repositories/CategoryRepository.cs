﻿using CMSWebsite.Data;
using CMSWebsite.Models;
using CMSWebsite.RepositoriesInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMSWebsite.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private AppDbContext _appDbContext;

        public CategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Create(Category model)
        {
            _appDbContext.Categories.Add(model);
            _appDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            try
            {
                Category category = _appDbContext.Categories.Find(id);
                _appDbContext.Categories.Remove(category);
                _appDbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        public void Edit(Category model)
        {
            try
            {
                _appDbContext.Entry(model).State = EntityState.Modified;
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IEnumerable<Category> GetAll()
        {
            try
            {
                return _appDbContext.Categories.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public Category GetById(int id)
        {
            try
            {
                return _appDbContext.Categories.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
