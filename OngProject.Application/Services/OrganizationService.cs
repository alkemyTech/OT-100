﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OngProject.Application.DTOs.Organizations;
using OngProject.Application.Exceptions;
using OngProject.DataAccess.Interfaces;
using OngProject.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Application.Services
{
    public class OrganizationService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrganizationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetOrganizationsDto>> GetAll()
        {
            var organizations = await _unitOfWork.Organizations.GetAll();
            return organizations
                .AsQueryable()
                //.Where(m => m.DeletedAt == null) Hay que agregar la logica para las entidades eliminadas?
                .AsNoTracking()
                .ProjectTo<GetOrganizationsDto>(_mapper.ConfigurationProvider);
        }

        public async Task<GetOrganizationDetailsDto> GetById(int id)
        {
            var organization = await _unitOfWork.Organizations.GetById(id);

            if (organization is null)// || organization.DeletedAt != null)
                throw new NotFoundException(nameof(Organization), id);

            return _mapper.Map<GetOrganizationDetailsDto>(organization);
        }

        public async Task<GetOrganizationsDto> CreateOrganization(CreateOrganizationDto createOrganizationDto)
        {
            var organization = _mapper.Map<Organization>(createOrganizationDto);

            await _unitOfWork.Organizations.Create(organization);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<GetOrganizationsDto>(organization);
        }

        public async Task Update(int id, CreateOrganizationDto updateOrganization)
        {
            var organization = await _unitOfWork.Organizations.GetById(id);

            if (organization is null)
                throw new NotFoundException(nameof(Organization), id);

            await _unitOfWork.Organizations.Update(_mapper.Map(updateOrganization, organization));
            await _unitOfWork.CompleteAsync();

        }

        public async Task Delete(int id)
        {
            var organization = await _unitOfWork.Organizations.GetById(id);

            if (organization is null)
                throw new NotFoundException(nameof(Organization), id);

            await _unitOfWork.Organizations.Delete(organization);
            await _unitOfWork.CompleteAsync();

        }

    }
}

