export const sessionMemory = {
   get(key: string): any {
      const value = typeof window !== 'undefined' ? sessionStorage.getItem(key) : null;
      if (value) return JSON.parse(value);
   },

   set(key: string, value: any) {
      sessionStorage.setItem(key, JSON.stringify(value));
   },

   remove(key: string) {
      sessionStorage.removeItem(key);
   },
};
