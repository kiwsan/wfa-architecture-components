using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq.Expressions;

//https://www.carbonatethis.com/async-query-and-batch-with-entity-framework/
namespace Common.Data
{
    /*
    var batch = AsyncBatch<DataContext>.Create();
    batch.Queue(c => c.Companies.Add(new Company() { Name = "Bobs Burgers" }));
    batch.Queue(c => c.Companies.Add(new Company() { Name = "Taco Johns" }));
    batch.Queue(c => c.Companies.Add(new Company() { Name = "Wendys" }));
    batch.Queue(c => c.Companies.Add(new Company() { Name = "Super Burger" }));
    batch.Queue(c => c.Companies.Add(new Company() { Name = "Nacho Mamas" }));
    batch.Queue(c => c.SaveChanges());

    batch.ExecuteAsync(() =>
    {
        using (var context = new DataContext())
        {
            this.Companies = context.Companies.ToList();
        }
    });
    */

    public class AsyncBatch<T> where T : DbContext, new()
    {
        /// <summary>
        /// Creates a new instance of <see cref="AsyncBatch"/> with the given DbContext
        /// </summary>
        public static AsyncBatch<T> Create()
        {
            var unitOfWork = new AsyncBatch<T>();
            unitOfWork.context = new T();
            unitOfWork.actions = new List<Expression<Action<T>>>();
            return unitOfWork;
        }

        /// <summary>
        /// Execute all actions that have been queued asynchronously
        /// </summary>
        /// <param name="callback">The method to execute when all actions have been completed</param>
        /// <param name="exceptionCallback">The method to exeute if there was an unhandled exception</param>
        public void ExecuteAsync(Action callback, Action<Exception> exceptionCallback = null)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                foreach (var action in this.actions)
                    action.Compile()(this.context);
            };
            worker.RunWorkerCompleted += (s, e) =>
            {
                context.Dispose();

                if (e.Error == null && callback != null)
                    callback();
                else if (e.Error != null && exceptionCallback != null)
                    exceptionCallback(e.Error);
            };
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Queue an action to be executed asynchronously
        /// </summary>
        /// <param name="action">The action</param>
        public AsyncBatch<T> Queue(Expression<Action<T>> action)
        {
            actions.Add(action);
            return this;
        }

        private List<Expression<Action<T>>> actions;
        private T context;
    }
}
