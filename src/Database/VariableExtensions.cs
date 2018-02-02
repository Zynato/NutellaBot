using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Database
{
    public static class VariableExtensions
    {
        public static int GetUserVariableAsInt32(this IVariableStorage variables, ulong userId, string variable) {
            return CastMaybeStringToInt(variables.GetUserVariable(userId, variable));
        }

        public static void SetUserVariable(this IVariableStorage variables, ulong userId, string variable, int value) {
            variables.SetUserVariable(userId, variable, value.ToString());
        }

        public static int GetGlobalVariableAsInt32(this IVariableStorage variables, string variable) {
            return CastMaybeStringToInt(variables.GetGlobalVariable(variable));
        }

        public static void SetGlobalVariable(this IVariableStorage variables, string variable, int value) {
            variables.SetGlobalVariable(variable, value.ToString());
        }

        public static bool GetUserVariableAsBoolean(this IVariableStorage variables, ulong userId, string variable) {
            return CastMaybeStringToBoolean(variables.GetUserVariable(userId, variable));
        }

        public static void SetUserVariable(this IVariableStorage variables, ulong userId, string variable, bool value) {
            variables.SetUserVariable(userId, variable, value.ToString());
        }

        public static bool GetGlobalVariableAsBoolean(this IVariableStorage variables, string variable) {
            return CastMaybeStringToBoolean(variables.GetGlobalVariable(variable));
        }

        public static void SetGlobalVariable(this IVariableStorage variables, string variable, bool value) {
            variables.SetGlobalVariable(variable, value.ToString());
        }

        private static int CastMaybeStringToInt(Maybe<string> maybeValue) {
            int value = 0;
            if (maybeValue.HasValue) {
                value = Convert.ToInt32(maybeValue.Value);
            }

            return value;
        }

        private static bool CastMaybeStringToBoolean(Maybe<string> maybeValue) {
            bool value = false;
            if (maybeValue.HasValue) {
                value = Convert.ToBoolean(maybeValue.Value);
            }

            return value;
        }
    }
}
