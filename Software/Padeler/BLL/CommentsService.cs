using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public sealed class CommentsService // Kristian Katulić
    {
        private readonly ICommentsRepository _repo;
        public const int MaxCommentLength = 250;
        public CommentsService() : this(new CommentsRepository())
        {
        }
        public CommentsService(ICommentsRepository repo)
        {
            _repo = repo;
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

            if (comment != null && comment.Length > MaxCommentLength)
                throw new ArgumentException($"Comment can't be longer than {MaxCommentLength} characters.");

            var ratedIds = await _repo.GetRatedIdsAsync(commenterId);
            if (ratedIds != null && ratedIds.Contains(commentedId))
                throw new InvalidOperationException("You have already graded this user.");

            return await _repo.AddRatingAsync(commentedId, commenterId, grade, comment);
        }

        /// <summary>
        /// Returns how many characters are remaining for a comment.
        /// Null, empty or whitespace comments are treated as 0 characters.
        /// The comment is trimmed before calculating the length.
        /// The result can be negative when the comment is too long.
        /// </summary>
        public int GetRemainingCommentCharacters(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
                return MaxCommentLength;

            int length = comment.Trim().Length;
            return MaxCommentLength - length;
        }

        /// <summary>
        /// Returns true when the trimmed comment exceeds the maximum allowed length.
        /// </summary>
        public bool IsCommentTooLong(string comment)
        {
            return GetRemainingCommentCharacters(comment) < 0;
        }
    }
}
