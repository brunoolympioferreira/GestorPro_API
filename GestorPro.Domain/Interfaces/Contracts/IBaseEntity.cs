using System;
using System.Collections.Generic;
using System.Text;

namespace GestorPro.Domain.Interfaces.Contracts;

public interface IBaseEntity
{
    Guid Id { get; set; }
}
