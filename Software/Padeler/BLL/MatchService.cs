using DAL;
using EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MatchService : IMatchUserService
    {
        private readonly MatchesRepository _matchRepo;
        private readonly MatchListRepository _repo;

        public MatchService()
        {
            _matchRepo = new MatchesRepository();
            _repo = new MatchListRepository();
        }
        /// <summary>
        /// Registrira "LIKE" akciju između dva korisnika.
        /// Ako oba korisnika međusobno označe "LIKE", metoda vraća true
        /// čime se signalizira da je došlo do matcha.
        /// </summary>
        public async Task<bool> LikeAsync(int fromUserId, int toUserId) // Filip Grgac
        {
            var res = await _matchRepo.SwipeAsync(fromUserId, toUserId, "LIKE");

            if(res == null)
            {
                throw new Exception("Nema odgovora od API-ja");
            }

            if (!res.Success)
            {
                throw new Exception(res.Error);
            }

            return res.Success && res.Matched;
        }

        /// <summary>
        /// Registrira "DISLIKE" akciju između dva korisnika.
        /// Ova akcija ne može rezultirati matchom i ne vraća povratnu vrijednost.
        /// </summary>
        public async Task DislikeAsync(int fromUserId, int toUserId) // Filip Grgac
        {
            var res = await _matchRepo.SwipeAsync(fromUserId, toUserId, "DISLIKE");

            if (res == null)
            {
                throw new Exception("Nema odgovora od API-ja");
            }

            if (!res.Success)
            {
                throw new Exception(res.Error);
            }
        }

        /// <summary>
        /// Dohvaća sve matchove za zadanog korisnika,
        /// odnosno popis korisnika s kojima je ostvaren međusobni match.
        /// </summary>
        public async Task<List<MatchDto>> GetMyMatchesAsync(int userId)
        {
            var res = await _repo.GetMyMatchesAsync(userId);
            return res;
        }

        /// <summary>
        /// Dohvaća sve matchove korisnika zajedno s pripadajućim
        /// korisničkim postavkama (nickname, skrivenost).
        /// Skriveni match entryji se ne uključuju u rezultat.
        /// </summary>
        public async Task<List<MatchRow>> GetMatchedEntries(int userId) // Filip Grgac
        {
            var matches = await _repo.GetMyMatchesAsync(userId);
            var entries = await _repo.FindAllForUsersAsync(userId);

            var entryByMatches = entries.GroupBy(e => e.MatchedUserId).ToDictionary(g => g.Key, g => g.First());

            var rows = new List<MatchRow>();

            foreach(var m in matches)
            {
                entryByMatches.TryGetValue(m.OtherUserId, out var entry);

                if(entry != null && entry.IsHidden)
                {
                    continue;
                }

                rows.Add(CreateMatchRow(m, entry));
            }

            return rows;
        }

        private MatchRow CreateMatchRow(MatchDto match, MatchEntryDto entry)
        {
            return new MatchRow
            {
                MatchId = match.MatchId,
                OtherUserId = match.OtherUserId,
                FullName = $"{match.OtherName} {match.OtherSurname}".Trim(),
                Phone = match.OtherPhone ?? "",
                Nickname = entry?.CustomNickname ?? ""
            };
        }

        /// <summary>
        /// Dohvaća detalje match entryja između trenutnog korisnika
        /// i određenog matched korisnika.
        /// </summary>
        public async Task<MatchEntryDto> GetEntry(int currentUserId, int matchedUserId) // Filip Grgac
        {
            return await _repo.FindByUsersAsync(currentUserId, matchedUserId);
        }

        /// <summary>
        /// Ažurira podatke match entryja (npr. nadimak)
        /// za određenog matched korisnika.
        /// </summary>
        public async Task<bool> UpdateEntry(MatchEntryDto entry) // Filip Grgac
        {
            await _repo.SaveAsync(entry);
            return true;
        }

        /// <summary>
        /// Logički uklanja (skriva) match entry za korisnika,
        /// bez brisanja zapisa iz baze podataka.
        /// </summary>
        public async Task<bool> DeleteEntry(MatchEntryDto entry) // Filip Grgac
        {
            await _repo.HideAsync(entry.CurrentUserId, entry.MatchedUserId);
            return true;
        }
    }
}
