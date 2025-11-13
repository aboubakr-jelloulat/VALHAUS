< !DOCTYPE html >
    <html lang="en">
        <head>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <title>@ViewData["Title"] - VALHÄUS</title>

            <!-- Bootstrap (existing) -->
            <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />

            <!-- Nimbus CSS -->
            <link rel="stylesheet" href="~/css/nimbus/bootstrap.min.css" />
            <link rel="stylesheet" href="~/css/nimbus/onepage.css" />
            <link rel="stylesheet" href="~/css/nimbus/vendors/aos.css" />

            <!-- Bootstrap Icons -->
            <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css">

                <!-- Custom site CSS -->
                <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
                <link rel="stylesheet" href="~/VALHÄUS.styles.css" asp-append-version="true" />

                <!-- Dark Mode Toggle Styles -->
                <style>
        /* Smooth theme transition */
                    * {
                        transition: background-color 0.3s ease, color 0.3s ease, border-color 0.3s ease;
        }

                    /* Modern Toggle Switch Container */
                    .theme-toggle-wrapper {
                        display: flex;
                    align-items: center;
                    gap: 8px;
        }

                    /* Toggle Switch */
                    .theme-switch {
                        position: relative;
                    display: inline-block;
                    width: 64px;
                    height: 32px;
        }

                    .theme-switch input {
                        opacity: 0;
                    width: 0;
                    height: 0;
        }

                    /* Slider */
                    .theme-slider {
                        position: absolute;
                    cursor: pointer;
                    top: 0;
                    left: 0;
                    right: 0;
                    bottom: 0;
                    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                    border-radius: 34px;
                    transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
                    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        }

                    .theme-slider:before {
                        position: absolute;
                    content: "";
                    height: 24px;
                    width: 24px;
                    left: 4px;
                    bottom: 4px;
                    background-color: white;
                    border-radius: 50%;
                    transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
                    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
        }

                    /* Icon inside slider */
                    .theme-slider i {
                        position: absolute;
                    top: 50%;
                    transform: translateY(-50%);
                    font-size: 14px;
                    transition: all 0.4s ease;
        }

                    .sun-icon {
                        left: 8px;
                    color: #fff;
                    opacity: 1;
        }

                    .moon-icon {
                        right: 8px;
                    color: #fff;
                    opacity: 0;
        }

                    /* Checked state (Dark Mode) */
                    input:checked + .theme-slider {
                        background: linear-gradient(135deg, #2c3e50 0%, #34495e 100%);
        }

                    input:checked + .theme-slider:before {
                        transform: translateX(32px);
        }

                    input:checked + .theme-slider .sun-icon {
                        opacity: 0;
        }

                    input:checked + .theme-slider .moon-icon {
                        opacity: 1;
        }

                    /* Hover effect */
                    .theme-slider:hover {
                        box - shadow: 0 6px 16px rgba(0, 0, 0, 0.25);
        }

                    /* Dark mode navbar adjustments */
                    [data-bs-theme="dark"] .navbar {
                        background - color: #1a1a1a !important;
                    border-bottom-color: #333 !important;
        }

                    [data-bs-theme="dark"] .navbar-brand,
                    [data-bs-theme="dark"] .nav-link {
                        color: #f8f9fa !important;
        }

                    [data-bs-theme="dark"] .footer {
                        background - color: #1a1a1a;
                    color: #adb5bd;
                    border-top-color: #333 !important;
        }

                    [data-bs-theme="dark"] body {
                        background - color: #121212;
                    color: #e0e0e0;
        }
                </style>

                @RenderSection("Styles", required: false)
        </head>
        <body>
            <header>
                <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
                    <div class="container-fluid">
                        <a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">VALHÄUS</a>
                        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                            aria-expanded="false" aria-label="Toggle navigation">
                            <span class="navbar-toggler-icon"></span>
                        </button>
                        <div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                            <ul class="navbar-nav flex-grow-1">
                                <li class="nav-item">
                                    <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link text-dark" asp-area="" asp-controller="Category" asp-action="Index">Category</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
                                </li>

                                <!-- Modern Dark Mode Toggle Switch -->
                                <li class="nav-item d-flex align-items-center ms-3">
                                    <div class="theme-toggle-wrapper">
                                        <label class="theme-switch">
                                            <input type="checkbox" id="theme-toggle">
                                                <span class="theme-slider">
                                                    <i class="bi bi-sun-fill sun-icon"></i>
                                                    <i class="bi bi-moon-stars-fill moon-icon"></i>
                                                </span>
                                        </label>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                </nav>
            </header>

            <div class="container">
                <main role="main" class="pb-3">
                    @RenderBody()
                </main>
            </div>

            <footer class="border-top footer text-muted">
                <div class="container">
                    &copy; 2025 - VALHÄUS - <a asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
                </div>
            </footer>

            <!-- jQuery -->
            <script src="~/lib/jquery/dist/jquery.min.js"></script>

            <!-- Bootstrap JS -->
            <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>

            <!-- Nimbus JS -->
            <script src="~/js/nimbus/vendors/aos.js"></script>
            <script src="~/js/nimbus/custom.js"></script>

            <!-- Dark Mode Toggle Script -->
            <script src="~/js/darkmode-toggle.js"></script>

            <!-- Site JS -->
            <script src="~/js/site.js" asp-append-version="true"></script>

            @await RenderSectionAsync("Scripts", required: false)
        </body>
    </html>