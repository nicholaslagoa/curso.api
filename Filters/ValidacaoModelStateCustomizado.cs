using curso.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Filters
{
    public class ValidacaoModelStateCustomizado : ActionFilterAttribute//utiliza essa linha de código do mvc pra fazer um filtro custom
    {
        public override void OnActionExecuting(ActionExecutingContext context)//override pq a classe é abstrata normal
        {
            if (!context.ModelState.IsValid)
            {
                var validaCampoViewModel = new ValidaCampoViewModelOutput(context.ModelState.SelectMany(sm => sm.Value.Errors).Select(s => s.ErrorMessage));//mesma coisa, percorre todos os models, todos os erros e mensagens
                context.Result = new BadRequestObjectResult(validaCampoViewModel);//manda uma lista de erros
            }
        }
    }
}
