export type PasswordStrengthLevel = 'Weak' | 'Medium' | 'Strong';

export function getPasswordStrength(password: string): {
  level: PasswordStrengthLevel;
  score: number;
  className: string;
} {
  let score = 0;

  if (password.length >= 8) score++;
  if (/[A-Z]/.test(password)) score++;
  if (/[a-z]/.test(password)) score++;
  if (/\d/.test(password)) score++;
  if (/[^A-Za-z0-9]/.test(password)) score++;

  if (score <= 2) {
    return {
      level: 'Weak',
      score,
      className: 'bg-red-500'
    };
  }

  if (score <= 4) {
    return {
      level: 'Medium',
      score,
      className: 'bg-yellow-500'
    };
  }

  return {
    level: 'Strong',
    score,
    className: 'bg-emerald-500'
  };
}