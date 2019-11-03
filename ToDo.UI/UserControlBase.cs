using System;
using System.Windows.Controls;
using ToDo.Core.Binders;
using ToDoUICore.AppService;

namespace ToDo.UI
{
    /// <summary>
    /// UserControlBase
    /// </summary>
    public class UserControlBase : UserControl
    {
        #region fields

        protected object Model;

        #endregion fields

        #region methods

        /// <summary>
        /// Create ViewModel instance and bind to view
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async void BindAsync<T>() where T : IModelInitilizer
        {
            var modelInitlizer = (await DependencyServiceSingleton.Instance.GetAsync<T>());

            if (modelInitlizer == null)
                throw new Exception("Can't create instance");

            await modelInitlizer.InitlizeAsync();
            Model = modelInitlizer.Model;
            DataContext = Model;
        }

        #endregion methods
    }
}