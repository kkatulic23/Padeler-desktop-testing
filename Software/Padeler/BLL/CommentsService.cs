using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public sealed class CommentsService
    {
        private readonly CommentsRepository _repo;

        public CommentsService()
        {
            _repo = new CommentsRepository();
        }

        /// <summary>
        /// Dodaje ocjenu i komentar za drugog korisnika.
        /// Provjerava ispravnost podataka, onemogućava ocjenjivanje samog sebe
        /// te sprječava višestruko ocjenjivanje istog korisnika.
        /// </summary>
        /// <param name="commentedId">ID korisnika koji se ocjenjuje</param>
        /// <param name="commenterId">ID korisnika koji ocjenjuje</param>
        /// <param name="grade">Ocjena od 1 do 5</param>
        /// <param name="comment">Opcionalni komentar</param>
        /// <returns>Rezultat dodavanja ocjene</returns>
        public async Task<int> AddRatingAsync(int commentedId, int commenterId, int grade, string comment)
        {
            if (commentedId <= 0 || commenterId <= 0)
                throw new ArgumentException("Bad user IDs.");

            if (commentedId == commenterId)
                throw new ArgumentException("U can't grade yourself.");

            if (grade < 1 || grade > 5)
                throw new ArgumentException("Grade has to be between 1 and 5.");

            comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();

            var ratedIds = await _repo.GetRatedIdsAsync(commenterId);
            if (ratedIds.Contains(commentedId))
                throw new InvalidOperationException("You have already graded this user.");

            return await _repo.AddRatingAsync(commentedId, commenterId, grade, comment);
        }
    }
}
