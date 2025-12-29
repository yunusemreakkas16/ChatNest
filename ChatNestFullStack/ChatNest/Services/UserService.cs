using ChatNest.Models.Common;
using ChatNest.Models.Domain;
using ChatNest.Models.DTO;
using ChatNest.Repositories;
using ChatNest.Utils;

namespace ChatNest.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<UserResponseModel> CreateUserAsync(CreateUserRequestDto createUserDto)
        {
            var hashedPassword = PasswordHasher.HashPassword(createUserDto.PasswordHash);

            var user = new User
            {
                username = createUserDto.Username,
                email = createUserDto.Email,
                passwordHash = hashedPassword,
            };
            return await userRepository.CreateUserAsync(user);
        }

        public async Task<UserResponseModelDetailed> GetUserDetailedAsync(UserParamModel userParam)
        {
            return await userRepository.GetUserDetailedAsync(userParam);
        }

        public async Task<UserResponseModelDetailed> UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto)
        {
            // Hash if value is not null or empty
            string? hashedPassword = string.IsNullOrEmpty(updateUserRequestDto.PasswordHash)? null: PasswordHasher.HashPassword(updateUserRequestDto.PasswordHash);

            var user = new User
            {
                userID = updateUserRequestDto.UserID,
                username = updateUserRequestDto.Username, // may be null
                email = updateUserRequestDto.Email,       // may be null
                passwordHash = hashedPassword             // may be null
            };

            return await userRepository.UpdateUserAsync(user);
        }
        public async Task<BaseResponse> SoftDeleteUserAsync(UserParamModel userParam)
        {
            return await userRepository.SoftDeleteUserAsync(userParam);
        }
        public async Task<UserResponseModelDetailed> ReActivateUserAsync(UserParamModel userParam)
        {
            var user = await userRepository.ReActivateUserAsync(userParam);
            return user;
        }

        public async Task<UserIDResponseModel> GetUsersByEmailsAsync(GetIDsByEmailRequestsDto getIDByEmailRequestDto)
        {
            if (getIDByEmailRequestDto == null || getIDByEmailRequestDto.Email == null || !getIDByEmailRequestDto.Email.Any())
            {
                return new UserIDResponseModel
                {
                    MessageID = -2,
                    MessageDescription = "Email list cannot be null or empty.",
                    UserIDResponse = new List<UserIDResponse>()
                };
            }

            var response = await userRepository.GetUserIDsByMailsAsync(getIDByEmailRequestDto);

            return response;
        }

        public async Task<UserIDResponseModel> GetUserByEmailFailedAsync(string email)
        {
            return await userRepository.GetUserByEmailFailedAsync(email);
        }
    }
}
