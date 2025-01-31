﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Acetesa.TomaPedidos.AdminMvc.ActionResults;
using Acetesa.TomaPedidos.AdminMvc.Filters;
using Acetesa.TomaPedidos.Transversal.Enums;
using Microsoft.Web.Mvc;

namespace Acetesa.TomaPedidos.AdminMvc.Infrastructure
{
	[IssueTypeSelectListPopulator, 
	 UserSelectListPopulator]
	public abstract class BaseController : Controller
	{
        public SelectList NewSelectList(IEnumerable lista)
        {
            if (lista == null)
            {
                lista = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = FindTypes.Ninguno.ToString(),
                        Value = FindTypes.Ninguno.ToString(),
                        Selected = true
                    }
                };
            }
            return new SelectList(lista, "Value", "Text");
        }
		protected ActionResult RedirectToAction<TController>(Expression<Action<TController>> action)
			where TController : Controller
		{
			return ControllerExtensions.RedirectToAction(this, action);
		}

		[Obsolete("Do not use the standard Json helpers to return JSON data to the client.  Use either JsonSuccess or JsonError instead.")]
		protected JsonResult Json<T>(T data)
		{
			throw new InvalidOperationException("Do not use the standard Json helpers to return JSON data to the client.  Use either JsonSuccess or JsonError instead.");
		}

		protected StandardJsonResult JsonValidationError()
		{
			var result = new StandardJsonResult();

			foreach (var validationError in ModelState.Values.SelectMany(v => v.Errors))
			{
				result.AddError(validationError.ErrorMessage);
			}
			return result;
		}

		protected StandardJsonResult JsonError(string errorMessage)
		{
			var result = new StandardJsonResult();

			result.AddError(errorMessage);

			return result;
		}

		protected StandardJsonResult<T> JsonSuccess<T>(T data)
		{
			return new StandardJsonResult<T> { Data = data };
		}
	}
}