using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Dto
{
    public record CreateProductRequest(string Name, decimal Price);
}
