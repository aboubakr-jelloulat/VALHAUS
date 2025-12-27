/**
 * VALHAUS Simple Dark Mode Toggle
 * Circular Button with Icon Change
 * Author: codew4re
 * Date: 2025-11-06
 */

/**
 * VALHAUS Dark Mode Toggle (Bootstrap 5.3)
 * Author: codew4re
 */

(() => {
    'use strict';

    const STORAGE_KEY = 'valhaus-theme';

    const getStoredTheme = () => localStorage.getItem(STORAGE_KEY);
    const setStoredTheme = (theme) => localStorage.setItem(STORAGE_KEY, theme);

    const getPreferredTheme = () => {
        return getStoredTheme()
            ?? (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');
    };

    const setTheme = (theme) => {
        document.documentElement.setAttribute('data-bs-theme', theme);
        updateIcon(theme);
    };

    const updateIcon = (theme) => {
        const icon = document.querySelector('#theme-toggle .theme-icon');
        if (!icon) return;

        icon.className = theme === 'dark'
            ? 'bi bi-moon-stars-fill theme-icon'
            : 'bi bi-sun-fill theme-icon';
    };

    const toggleTheme = () => {
        const newTheme = document.documentElement.getAttribute('data-bs-theme') === 'dark'
            ? 'light'
            : 'dark';

        setStoredTheme(newTheme);
        setTheme(newTheme);
    };

    // Init
    document.addEventListener('DOMContentLoaded', () => {
        setTheme(getPreferredTheme());

        const toggleBtn = document.getElementById('theme-toggle');
        if (toggleBtn) {
            toggleBtn.addEventListener('click', toggleTheme);
        }
    });

    // System theme sync (only if user didn't choose manually)
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', e => {
        if (!getStoredTheme()) {
            setTheme(e.matches ? 'dark' : 'light');
        }
    });
})();
