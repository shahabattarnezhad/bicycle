﻿using Application.Contracts.Base;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using System.Linq.Expressions;

namespace Repository.Base;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected ApplicationDbContext _context;

    public RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ? _context.Set<T>().AsNoTracking() :
                        _context.Set<T>();


    public IQueryable<T> FindByCondition(Expression<Func<T, bool>>
                     expression, bool trackChanges) =>
        !trackChanges ? _context.Set<T>()
                                         .Where(expression)
                                         .AsNoTracking()
                      : _context.Set<T>()
                                         .Where(expression);


    public void Create(T entity) => _context.Set<T>().Add(entity);


    public void Update(T entity) => _context.Set<T>().Update(entity);


    public void Delete(T entity) => _context.Set<T>().Remove(entity);
}
