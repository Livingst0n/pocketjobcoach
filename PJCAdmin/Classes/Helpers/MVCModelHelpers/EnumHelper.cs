using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.MVCModelHelpers
{
    public class EnumHelper
    {
        private pjcEntities db = new pjcEntities();

        #region TaskCategory
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
        public List<TaskCategory> getAllTaskCategories()
        {
            return db.TaskCategories.ToList();
        }
        public TaskCategory getTaskCategory(string category)
        {
            if (!taskCategoryExists(category))
                return null;

            return db.TaskCategories.Where(c => c.categoryName.Equals(category)).First();
        }
        public void updateTaskCategory(string oldCategory, string newCategory)
        {
            if (!taskCategoryExists(oldCategory))
            {
                createTaskCategory(newCategory);
                return;
            }

            if (taskCategoryExists(newCategory))
                return; //TODO model error

            TaskCategory tc = getTaskCategory(oldCategory);
            tc.categoryName = newCategory;

            db.Entry<TaskCategory>(tc).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }
        public void deleteTaskCategory(string categoryName)
        {
            if (!taskCategoryExists(categoryName))
                return;

            if (taskCategoryHasTasks(categoryName))
                return; //TODO model error

            db.TaskCategories.Remove(getTaskCategory(categoryName));
            db.SaveChanges();
        }
        public bool taskCategoryExists(string categoryName)
        {
            return db.TaskCategories.Where(c => c.categoryName.Equals(categoryName)).Count() > 0;
        }
        private bool taskCategoryHasTasks(string categoryName)
        {
            if (!taskCategoryExists(categoryName))
                return false;

            return getTaskCategory(categoryName).Tasks.Count() > 0;
        }
        #endregion

        #region MediaType
        /* create MediaType is managed in code
         * public void createMediaType(string type) 
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
        public List<MediaType> getAllMediaTypes() 
        {
            return db.MediaTypes.ToList();
        }
        public MediaType getMediaType(string type) 
        {
            if (!mediaTypeExists(type))
                return null;

            return db.MediaTypes.Where(t => t.mediaTypeName.Equals(type)).First();
        }
        /* update MediaType is managed in code
         * public void updateMediaType(string oldType, string newType) 
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
        /* delete MediaType is managed in code
         * public void deleteMediaType(string typeName) 
        {
            if (!mediaTypeExists(typeName))
                return;

            if (mediaTypeHasFeedbacks(typeName))
                return; //TODO model error

            db.MediaTypes.Remove(getMediaType(typeName));
            db.SaveChanges();
        }*/
        public bool mediaTypeExists(string typeName) 
        {
            return db.MediaTypes.Where(t => t.mediaTypeName.Equals(typeName)).Count() > 0;
        }
        public bool mediaTypeHasFeedbacks(string typeName) 
        {
            if (!mediaTypeExists(typeName))
                return false;

            return getMediaType(typeName).Feedbacks.Count() > 0;
        }
        #endregion

        #region FeedbackType
        /* create FeedbackType is managed in code
         * public void createFeedbackType(string type) 
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
        public List<FeedbackType> getAllFeedbackTypes() 
        {
            return db.FeedbackTypes.ToList();
        }
        public FeedbackType getFeedbackType(string type) 
        { 
            if (!feedbackTypeExists(type))
                return null;

            return db.FeedbackTypes.Where(t => t.feedbackTypeName.Equals(type)).First();
        }
        /* update FeedbackType is managed in code
         * public void updateFeedbackType(string oldType, string newType) 
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
        /* delete FeedbackType is managed in code
         * public void deleteFeedbackType(string typeName) 
        {
            if (!feedbackTypeExists(typeName))
                return;

            if (feedbackTypeHasFeedbacks(typeName))
                return;

            db.FeedbackTypes.Remove(getFeedbackType(typeName));
            db.SaveChanges();
        }*/
        public bool feedbackTypeExists(string typeName) 
        {
            return db.FeedbackTypes.Where(t => t.feedbackTypeName.Equals(typeName)).Count() > 0;
        }
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