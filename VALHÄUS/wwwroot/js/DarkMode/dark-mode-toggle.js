/**
 * VALHÄUS Simple Dark Mode Toggle
 * Circular Button with Icon Change
 * Author: codew4re
 * Date: 2025-11-06
 */

(function () {
    'use strict';

    // ===== Storage Functions =====

    // Get the saved theme from browser localStorage
    const getStoredTheme = () => localStorage.getItem('valhaus-theme');

    // Save theme to browser localStorage
    const setStoredTheme = (theme) => localStorage.setItem('valhaus-theme', theme);

    // ===== Theme Detection =====

    // Get user's preferred theme (saved or system default)
    const getPreferredTheme = () => {
        const storedTheme = getStoredTheme();
        if (storedTheme) {
            return storedTheme;
        }

        // Check system preference
        const isDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;
        return isDarkMode ? 'dark' : 'light';
    };

    // ===== Apply Theme =====

    // Apply theme to the page
    const setTheme = (theme) => {
        document.documentElement.setAttribute('data-bs-theme', theme);
        updateIcon(theme);
        console.log('Theme applied:', theme);
    };

    // ===== Update Icon =====

    // Change icon based on current theme
    const updateIcon = (theme) => {
        const toggle = document.getElementById('theme-toggle');
        if (!toggle) return;

        const icon = toggle.querySelector('.theme-icon');
        if (!icon) return;

        if (theme === 'dark') {
            // Dark mode: show moon icon
            icon.className = 'bi bi-moon-stars theme-icon';
        } else {
            // Light mode: show sun icon
            icon.className = 'bi bi-sun-fill theme-icon';
        }
    };

    // ===== Toggle Theme =====

    // Switch between light and dark mode
    const toggleTheme = () => {
        const currentTheme = getStoredTheme() || getPreferredTheme();
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';

        console.log('Switching theme:', currentTheme, '→', newTheme);

        setStoredTheme(newTheme);
        setTheme(newTheme);
    };

    // ===== Initialize =====

    // Set up theme when page loads
    const initTheme = () => {
        const preferredTheme = getPreferredTheme();
        setTheme(preferredTheme);
        console.log('Dark mode initialized!');
    };

    // ===== Event Listeners =====

    // Initialize theme as soon as possible
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initTheme);
    } else {
        initTheme();
    }

    // Add click event to toggle button
    window.addEventListener('DOMContentLoaded', () => {
        const themeToggle = document.getElementById('theme-toggle');

        if (themeToggle) {
            themeToggle.addEventListener('click', toggleTheme);
            console.log('Theme toggle button ready! ✅');
        } else {
            console.error('Theme toggle button not found! ❌');
        }
    });

    // Listen for system theme changes
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', (e) => {
        // Only auto-change if user hasn't manually set a preference
        if (!getStoredTheme()) {
            const newTheme = e.matches ? 'dark' : 'light';
            setTheme(newTheme);
            console.log('System theme changed:', newTheme);
        }
    });

})();