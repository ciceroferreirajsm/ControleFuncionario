using ControleFuncionario.Domain.Entities;
using ControleFuncionario.Model;
using System.Collections;
using System.Collections.Generic;

namespace ControleFuncionario.Mapper;
public class FuncionarioProfile : AutoMapper.Profile
{
    public FuncionarioProfile()
    {
        CreateMap<Funcionario, FuncionarioDTO>().ReverseMap();
        CreateMap<Login, LoginDTO>().ReverseMap();
    }
}
