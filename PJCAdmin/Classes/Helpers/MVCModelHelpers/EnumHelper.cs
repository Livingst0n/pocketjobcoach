using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.MVCModelHelpers
{
    /* --------------------------------------------------------
     * The EnumHelper class provides common methods relating 
     * to enumerated type and category objects from the 
     * database for the MVC service.
     * Namely the TaskCategory, MediaType, and FeedbackType.
     * --------------------------------------------------------
     */
    public class EnumHelper
    {
        private pjcEntities db = new pjcEntities();

        #region TaskCategory
        /* Creates a new TaskCategory enum for the
         * given category. Noop if the category 
         * already exists.
         * @param category: The new category name.
         */
        public void createTaskCategory(string category)
        {
            if (taskCategoryExists(category))
                return; //TODO model error

            TaskCategory tc = new TaskCategory()
            {
                categoryName = category
            };

            db.TaskCategories.Add(tc);
            db.SaveChanges();
        }
        /* Returns a list of all categories in the 
         * TaskCategory enum.
         */
        public List<TaskCategory> getAllTaskCategories()
        {
            return db.TaskCategories.ToList();
        }
        /* Returns the TaskCategory record for the given
         * category.
         * @param category: The category name.
         */
        public TaskCategory getTaskCategory(string category)
        {
            if (!taskCategoryExists(category))
                return null;

            return db.TaskCategories.Where(c => c.categoryName.Equals(category)).First();
        }
        /* Changes the category name of the existing
         * enum record. Noop if the new category exists.
         * @param oldCategory: The category name of the 
         * existing TaskCategory.
         * @param newCategory: The new category name.
         */
        public void updateTaskCategory(string oldCategory, string newCategory)
        {
            if (taskCategoryExists(newCategory))
                return; //TODO model error

            if (!taskCategoryExists(oldCategory))
            {
                createTaskCategory(newCategory);
                return;
            }

            TaskCategory tc = getTaskCategory(oldCategory);
            tc.categoryName = newCategory;

            db.Entry<TaskCategory>(tc).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }
        /* Removes the TaskCategory record for the
         * given category. Noop if tasks exist for
         * the TaskCategory enum.
         * @param categoryName: The category name of 
         * the TaskCategory enum record to be deleted.
         */
        public void deleteTaskCategory(string categoryName)
        {
            if (!taskCategoryExists(categoryName))
                return;

            if (taskCategoryHasTasks(categoryName))
                return; //TODO model error

            db.TaskCategories.Remove(getTaskCategory(categoryName));
            db.SaveChanges();
        }
        /* Returns whether the TaskCategory enum contains
         * the given category.
         * @param categoryName: The category to be searched for.
         */
        public bool taskCategoryExists(string categoryName)
        {
            return db.TaskCategories.Where(c => c.categoryName.Equals(categoryName)).Count() > 0;
        }
        /* Returns whether the given category has been 
         * associated with any Tasks.
         * @param categoryName: The desired category to
         * be checked.
         */
        private bool taskCategoryHasTasks(string categoryName)
        {
            if (!taskCategoryExists(categoryName))
                return false;

            return getTaskCategory(categoryName).Tasks.Count() > 0;
        }
        #endregion
        #region MediaType
        /* create MediaType is code-managed*/
        /* public void createMediaType(string type) 
        {
            if (mediaTypeExists(type))
                return; //TODO model error

            MediaType mt = new MediaType()
            {
                mediaTypeName = type
            };

            db.MediaTypes.Add(mt);
            db.SaveChanges();
        }*/
        /* Returns a list of all types in the 
         * MediaType enum.
         */
        public List<MediaType> getAllMediaTypes() 
        {
            return db.MediaTypes.ToList();
        }
        /* Returns the MediaType record for the given
         * type.
         * @param type: The type name.
         */
        public MediaType getMediaType(string type) 
        {
            if (!mediaTypeExists(type))
                return null;

            return db.MediaTypes.Where(t => t.mediaTypeName.Equals(type)).First();
        }
        /* update MediaType is code-managed*/
        /* public void updateMediaType(string oldType, string newType) 
        {
            if (!mediaTypeExists(oldType))
            {
                createMediaType(newType);
                return;
            }

            if (mediaTypeExists(newType))
                return; //TODO model error

            MediaType mt = getMediaType(oldType);
            mt.mediaTypeName = newType;

            db.Entry<MediaType>(mt).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }*/
        /* delete MediaType is managed in code*/
        /* public void deleteMediaType(string typeName) 
        {
            if (!mediaTypeExists(typeName))
                return;

            if (mediaTypeHasFeedbacks(typeName))
                return; //TODO model error

            db.MediaTypes.Remove(getMediaType(typeName));
            db.SaveChanges();
        }*/
        /* Returns whether the MediaType enum contains
         * the given type.
         * @param typeName: The type to be searched for.
         */
        public bool mediaTypeExists(string typeName) 
        {
            return db.MediaTypes.Where(t => t.mediaTypeName.Equals(typeName)).Count() > 0;
        }
        /* Returns whether the given type has been
         * associated with any Feedbacks.
         * @param typeName: The desired type to be checked.
         */
        public bool mediaTypeHasFeedbacks(string typeName) 
        {
            if (!mediaTypeExists(typeName))
                return false;

            return getMediaType(typeName).Feedbacks.Count() > 0;
        }
        #endregion
        #region FeedbackType
        /* create FeedbackType is code-managed*/
        /* public void createFeedbackType(string type) 
        { 
            if (feedbackTypeExists(type))
                return; //TODO model error

            FeedbackType ft = new FeedbackType()
            {
                feedbackTypeName = type
            };

            db.FeedbackTypes.Add(ft);
            db.SaveChanges();
        }*/
        /* Returns a list of all types in the 
         * FeedbackType enum.
         */
        public List<FeedbackType> getAllFeedbackTypes() 
        {
            return db.FeedbackTypes.ToList();
        }
        /* Returns the FeedbackType record for the given
         * type.
         * @param type: The type name.
         */
        public FeedbackType getFeedbackType(string type) 
        { 
            if (!feedbackTypeExists(type))
                return null;

            return db.FeedbackTypes.Where(t => t.feedbackTypeName.Equals(type)).First();
        }
        /* update FeedbackType is managed in code*/
        /* public void updateFeedbackType(string oldType, string newType) 
        {
            if (!feedbackTypeExists(oldType))
            {
                createFeedbackType(newType);
                return;
            }

            if (feedbackTypeExists(newType))
                return; //TODO model error

            FeedbackType ft = getFeedbackType(oldType);
            ft.feedbackTypeName = newType;

            db.Entry<FeedbackType>(ft).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }*/
        /* delete FeedbackType is managed in code*/
        /* public void deleteFeedbackType(string typeName) 
        {
            if (!feedbackTypeExists(typeName))
                return;

            if (feedbackTypeHasFeedbacks(typeName))
                return;

            db.FeedbackTypes.Remove(getFeedbackType(typeName));
            db.SaveChanges();
        }*/
        /* Returns whether the FeedbackType enum contains
         * the given type.
         * @param typeName: The type to be searched for.
         */
        public bool feedbackTypeExists(string typeName) 
        {
            return db.FeedbackTypes.Where(t => t.feedbackTypeName.Equals(typeName)).Count() > 0;
        }
        /* Returns whether the given type has been
         * associated with any Feedbacks.
         * @param typeName: The desired type to be checked.
         */
        public bool feedbackTypeHasFeedbacks(string typeName) 
        {
            if (!feedbackTypeExists(typeName))
                return false;

            return getFeedbackType(typeName).Feedbacks.Count() > 0;
        }
        #endregion

        public void dispose()
        {
            db.Dispose();
        }
    }
}