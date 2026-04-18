using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Utils;

public static class ModelValidations
{
    public static void ValidateModel(this object model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);

        //  true → validate all properties
        //  false → validate only[Required]

        if (!isValid)
        {
            string errors = string.Join(" | ", results.Select(r => r.ErrorMessage));
            throw new ArgumentException(errors);
        }
    }

}
