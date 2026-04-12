using EL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public sealed class CommentsRepository
    {
        private readonly ApiClient _api = new ApiClient();

        /// <summary>
        /// Dodaje ocjenu (+ opcionalni komentar).
        /// PHP: require_post_fields(['commented_id','commenter_id','grade'])
        /// </summary>
        public async Task<int> AddRatingAsync(int commentedId, int commenterId, double grade, string comment = null) // Kristian Katulić
        {
            if (commentedId <= 0 || commenterId <= 0)
                throw new Exception("Invalid user ids.");

            if (grade < 1 || grade > 5)
                throw new Exception("Grade must be 1-5.");

            if (comment != null && comment.Trim() == "")
                comment = null;

            var req = new AddCommentRequest
            {
                CommentedId = commentedId,
                CommenterId = commenterId,
                Grade = grade,
                Comment = comment
            };

            var res = await _api.PostJsonAsync<AddCommentResponse>("api/comments/add_comment.php", req);

            if (res == null)
                throw new Exception("AddRating: API response is null.");

            if (!res.Success)
                throw new Exception(res.Error ?? "Failed to add comment.");

            return res.CommentId;
        }

        /// <summary>
        /// Dohvati listu userId-eva koje je commenter već ocijenio.
        /// PHP: GET ...get_rated_ids.php?commenter_id=#
        /// </summary>
        public async Task<List<int>> GetRatedIdsAsync(int commenterId) // Kristian Katulić
        {
            if (commenterId <= 0)
                throw new Exception("Invalid commenter_id.");

            var res = await _api.GetAsync<RatedIdsResponse>($"api/comments/my_rated.php?commenter_id={commenterId}");

            if (res == null)
                throw new Exception("GetRatedIds: API response is null.");

            if (!res.Success)
                throw new Exception(res.Error ?? "Failed to fetch rated ids.");

            return res.RatedIds ?? new List<int>();
        }
    }
}