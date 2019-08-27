using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

//https://www.carbonatethis.com/async-query-and-batch-with-entity-framework/
namespace Common.Data
{
    /*
    AsyncQuery<DataContext>.List<Company>(results =>
    {
        this.Companies = new ObservableCollection<Company>(results);
    });
    */

    public static class AsyncQuery<T> where T : DbContext, new()
    {
        /// <summary>
        /// Find an entity by type and id asynchronously
        /// </summary>
        public static void Find<TModel>(object[] keyValues, Action<TModel> callback, Action<Exception> exceptionCallback = null) where TModel : class
        {
            ExecuteAsync(() =>
            {
                using (var context = new T())
                {
                    return context.Set<TModel>().Find(keyValues);
                }
            }, callback, exceptionCallback);
        }

        /// <summary>
        /// Gets all entities of a given type asynchronously
        /// </summary>
        public static void List<TModel>(Action<IList<TModel>> callback, Action<Exception> exceptionCallback = null) where TModel : class
        {
            ExecuteAsync(() =>
            {
                using (var context = new T())
                {
                    return context.Set<TModel>().ToList();
                }
            }, callback, exceptionCallback);
        }

        /// <summary>
        /// Gets all entities of a given type asynchronously
        /// </summary>
        public static void List<TModel, TProperty>(Expression<Func<TModel, TProperty>> includePath, Action<IList<TModel>> callback, Action<Exception> exceptionCallback = null) where TModel : class
        {
            ExecuteAsync(() =>
            {
                using (var context = new T())
                {
                    return context.Set<TModel>().Include(includePath).ToList();
                }
            }, callback, exceptionCallback);
        }

        /// <summary>
        /// Get a list of filtered entities asynchronously
        /// </summary>
        public static void Where<TModel>(Func<TModel, bool> predicate, Action<IEnumerable<TModel>> callback, Action<Exception> exceptionCallback = null) where TModel : class
        {
            ExecuteAsync(() =>
            {
                using (var context = new T())
                {
                    return context.Set<TModel>().Where<TModel>(predicate);
                }
            }, callback, exceptionCallback);
        }

        /// <summary>
        /// Get a list of filtered entities asynchronously
        /// </summary>
        public static void Where<TModel, TProperty>(Func<TModel, bool> predicate, Expression<Func<TModel, TProperty>> includePath, Action<IEnumerable<TModel>> callback, Action<Exception> exceptionCallback = null) where TModel : class
        {
            ExecuteAsync(() =>
            {
                using (var context = new T())
                {
                    return context.Set<TModel>().Include(includePath).Where<TModel>(predicate);
                }
            }, callback, exceptionCallback);
        }

        /// <summary>
        /// Execute a background task that performs its callback on the calling thread to avoid invoke.
        /// Requires the calling thread to have a synchronization context such as a WPF application.
        /// </summary>
        /// <typeparam name="TModel">The type of the result</typeparam>
        /// <param name="task">The method to execute async</param>
        /// <param name="callback">The callback</param>
        private static void ExecuteAsync<TModel>(Func<TModel> task, Action<TModel> callback, Action<Exception> exceptionCallback = null)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                e.Result = task();
            };
            worker.RunWorkerCompleted += (s, e) =>
            {
                if (e.Error == null && callback != null)
                    callback((TModel)e.Result);
                else if (e.Error != null && exceptionCallback != null)
                    exceptionCallback(e.Error);
            };
            worker.RunWorkerAsync();
        }
    }
}
