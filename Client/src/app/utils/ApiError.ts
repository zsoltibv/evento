export interface ApiError {
  code: string;
  description: string;
}

export function extractErrorMessages(error: any): string[] {
  const errors = ([] as ApiError[])
    .concat(error?.error ?? error ?? [])
    .filter((e) => e?.description);

  return errors.length > 0 ? errors.map((e) => e.description) : ['An unexpected error occurred'];
}
