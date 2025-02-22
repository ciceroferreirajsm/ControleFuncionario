using ControleFuncionario.Domain.Entities;
using ControleFuncionario.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ControleFuncionario.Mapper;
public class FuncionarioProfile : AutoMapper.Profile
{
    public FuncionarioProfile()
    {
        CreateMap<Funcionario, FuncionarioDTO>().ReverseMap();

        CreateMap<Login, LoginDTO>()
                 .ForMember(dest => dest.Permissao, opt => opt.MapFrom(src => src.Permissao.ToString()));

        CreateMap<LoginDTO, Login>()
            .ForMember(dest => dest.Permissao, opt => opt.MapFrom(src => BuildEnumParse(src.Permissao)
                ));
    }

    private Permissoes BuildEnumParse(string permissao)
    {
        if (Enum.TryParse<Permissoes>(permissao, true, out var permissaoOut))
            return permissaoOut;

        return Permissoes.funcionario;
    }
}
