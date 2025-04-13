   // Function to format a date as "Month Day, Year" (e.g., Mar 12, 2025)
        function formatDate(date) {
            const options = { year: 'numeric', month: 'short', day: 'numeric' };
            return date.toLocaleDateString('en-US', options);
        }

        // Get current date
        const today = new Date();

        // Format the current date
        const formattedToday = formatDate(today);

        // Set the formatted date into the span with id "date-range"
        document.getElementById('date-range').textContent = formattedToday;