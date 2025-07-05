using ApiBlog.Data;
using ApiBlog.Tag.DTO;
using ApiBlog.Tag.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBlog.Tag.Repository
{

    public class TagRepository : ITagRepository
    {
        private APIContext _context;
        public TagRepository(APIContext context)
        {
            _context = context;
        }

        public async Task<List<Models.Tag>> CadastraTag(List<string> tags)
        {
            if (tags == null || !tags.Any())
                return new List<Models.Tag>();

            var nomesNormalizados = tags
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => n.Trim().ToLower())
                .Distinct()
                .ToList();

            var tagsExistentes = await _context.Tags
                .Where(t => nomesNormalizados.Contains(t.Nome.ToLower()))
                .ToListAsync();

            var nomesExistentes = tagsExistentes.Select(t => t.Nome.ToLower()).ToList();
            var nomesParaCriar = nomesNormalizados.Except(nomesExistentes).ToList();

            var novasTags = nomesParaCriar.Select(nome => new Models.Tag
            {
                Nome = nome,
                Ativo = true
            }).ToList();

            if (novasTags.Any())
            {
                _context.Tags.AddRange(novasTags);
                await _context.SaveChangesAsync();
            }

            return tagsExistentes.Concat(novasTags).ToList();
        }
        public async Task<List<Models.Tag>> BuscarTagsPorNome(string? termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return await _context.Tags.Where(t => t.Ativo).ToListAsync();

             return await _context.Tags
                .Where(t => t.Ativo && t.Nome.Contains(termo))
                .ToListAsync();
        }
    }
}
