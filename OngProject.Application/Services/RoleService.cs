﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using OngProject.Application.DTOs.Roles;
using OngProject.DataAccess.Interfaces;
using OngProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngProject.Application.Services
{
    public class RoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetRolesDto>> GetRoles()
        {
            var roles = await _unitOfWork.Roles.GetAll();

            return roles
                .AsQueryable()
                .ProjectTo<GetRolesDto>(_mapper.ConfigurationProvider);
        }

        public async Task<GetRolesDto> GetRoleById(int id)
        {
            var role = await _unitOfWork.Roles.GetById(id);

            if (role is null)
                throw new Exception($"Entity was not found.");

            return _mapper.Map<GetRolesDto>(role);
        }

        public async Task<int> CreateRole(CreateRoleDto roleyDto)
        {
            var role = _mapper.Map<Role>(roleyDto);

            await _unitOfWork.Roles.Create(role);
            await _unitOfWork.CompleteAsync();

            return role.Id;
        }

        public async Task UpdateRole(int id, CreateRoleDto roleyDto)
        {
            var role = await _unitOfWork.Roles.GetById(id);

            if (role is null)
                throw new Exception($"Entity was not found.");

            role.Id = id;
            await _unitOfWork.Roles.Update(_mapper.Map(roleyDto, role));
            await _unitOfWork.CompleteAsync();
        }

        public async Task SoftDeleteRole(int id)
        {
            var role = await _unitOfWork.Roles.GetById(id);

            if (role is null)
                throw new Exception($"Entity was not found.");

            await _unitOfWork.Roles.Delete(role);
            await _unitOfWork.CompleteAsync();
        }
    }
}
