using AutoMapper;
using Dummy1.Books;
using Dummy1.CommuneDtos;
using Dummy1.DistrictDtos;
using Dummy1.HospitalDtos;
using Dummy1.Model;
using Dummy1.PantientDtos;
using Dummy1.ProvinceDtos;

namespace Dummy1;

public class Dummy1ApplicationAutoMapperProfile : Profile
{
    public Dummy1ApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();

        CreateMap<Province, ProvinceDto>();
        CreateMap<CreateUpdateProvinceDto, Province>();

        CreateMap<District, DistrictDto>();
        CreateMap<CreateUpdateDistrictDto, District>();

        CreateMap<Commune, CommuneDto>();
        CreateMap<CreateUpdateCommuneDto, Commune>();

        CreateMap<Hospital, HospitalDto>();
        CreateMap<CreateUpdateHospitalDto, Hospital>();

        CreateMap<Patient, PantientDto>();
        CreateMap<CreateUpdatePantientDto, Patient>();
    }
}
