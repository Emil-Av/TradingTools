using Models;
using Models.ViewModels.DisplayClasses;

namespace Utilities
{
    public class EntityMapper
    {
        /// <summary>
        ///  Converts the values from an entity to a display class.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TViewModelProperty"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static TViewModelProperty EntityToViewModel<TEntity, TViewModelProperty>(TEntity entity) where TViewModelProperty : new()
        {
            var currentTrade = new TViewModelProperty();
            var entityType = typeof(TEntity); 
            var viewModelPropType = typeof(TViewModelProperty);

            foreach (var entityProp in entityType.GetProperties())
            {
                var viewModelProp = viewModelPropType.GetProperty(entityProp.Name + "Display");
                if (viewModelProp != null)
                {
                    if (entityProp.PropertyType == typeof(bool) && viewModelProp.PropertyType == typeof(string))
                    {
                        var value = (bool)entityProp.GetValue(entity);
                        viewModelProp.SetValue(currentTrade, value ? "1" : "0");
                    }
                    else if (entityProp.PropertyType == typeof(int) && viewModelProp.PropertyType == typeof(string))
                    {
                        var value = entityProp.GetValue(entity)?.ToString();
                        viewModelProp.SetValue(currentTrade, value);
                    }
                    else
                    {
                        viewModelProp.SetValue(currentTrade, entityProp.GetValue(entity));
                    }
                }
            }

            return currentTrade;
        }

        /// <summary>
        ///  Converts the values from a display class to an entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TViewModelProperty"></typeparam>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static TEntity ViewModelToEntity<TEntity, TViewModelProperty>(TViewModelProperty viewModel) where TEntity : new()
        {
            var entity = new TEntity();
            var entityType = typeof(TEntity);
            var viewModelPropType = typeof(TViewModelProperty);

            foreach (var viewModelProp in viewModelPropType.GetProperties())
            {
                string VMPropName = viewModelProp.Name.Replace("Display", "");
                var entityProp = entityType.GetProperty(VMPropName);
                if (entityProp != null)
                {
                    if (viewModelProp.PropertyType == typeof(string) && entityProp.PropertyType == typeof(bool))
                    {
                        var value = (string)viewModelProp.GetValue(viewModel);
                        entityProp.SetValue(entity, value == "1");
                    }
                    else if (viewModelProp.PropertyType == typeof(string) && entityProp.PropertyType == typeof(int))
                    {
                        if (int.TryParse((string)viewModelProp.GetValue(viewModel), out int intValue))
                        {
                            entityProp.SetValue(entity, intValue);
                        }
                    }
                    else if (viewModelProp.PropertyType == typeof(string) && entityProp.PropertyType == typeof(double))
                    {
                        if (double.TryParse((string)viewModelProp.GetValue(viewModel), out double intValue))
                        {
                            entityProp.SetValue(entity, intValue);
                        }
                    } else if (viewModelProp.PropertyType == typeof(string) && entityProp.PropertyType == typeof(double?))
                    {
                        if (double.TryParse((string)viewModelProp.GetValue(viewModel), out double intValue))
                        {
                            entityProp.SetValue(entity, intValue);
                        }
                    }
                    else
                    {
                        entityProp.SetValue(entity, viewModelProp.GetValue(viewModel));
                    }
                }
            }

            return entity;
        }
    }
}
