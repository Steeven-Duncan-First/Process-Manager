// Gestion du chatbot
document.addEventListener('DOMContentLoaded', function() {
    const chatbotButton = document.querySelector('.chatbot-button');
    const chatbotWindow = document.querySelector('.chatbot-window');
    const chatbotMessages = document.querySelector('.chatbot-messages');
    const chatbotInput = document.querySelector('.chatbot-input input');
    const chatbotSendButton = document.querySelector('.chatbot-input button');

    if (chatbotButton && chatbotWindow) {
        chatbotButton.addEventListener('click', function() {
            chatbotWindow.classList.toggle('active');
        });

        // Fermer le chatbot en cliquant en dehors
        document.addEventListener('click', function(event) {
            if (!chatbotWindow.contains(event.target) && !chatbotButton.contains(event.target)) {
                chatbotWindow.classList.remove('active');
            }
        });

        // Envoyer un message
        function sendMessage() {
            const message = chatbotInput.value.trim();
            if (message) {
                addMessage(message, 'user');
                chatbotInput.value = '';

                // Appeler l'API pour obtenir la réponse
                fetch('/Procedure/ChatbotQuery', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(message)
                })
                .then(response => response.json())
                .then(data => {
                    if (data.length > 0) {
                        const proceduresList = data.map(p => `- ${p.title}`).join('\n');
                        addMessage(`Voici les procédures trouvées :\n${proceduresList}`, 'bot');
                    } else {
                        addMessage('Je n\'ai trouvé aucune procédure correspondant à votre demande.', 'bot');
                    }
                })
                .catch(error => {
                    console.error('Erreur:', error);
                    addMessage('Désolé, une erreur est survenue. Veuillez réessayer.', 'bot');
                });
            }
        }

        if (chatbotSendButton) {
            chatbotSendButton.addEventListener('click', sendMessage);
        }

        if (chatbotInput) {
            chatbotInput.addEventListener('keypress', function(event) {
                if (event.key === 'Enter') {
                    sendMessage();
                }
            });
        }

        function addMessage(text, type) {
            const messageDiv = document.createElement('div');
            messageDiv.className = `message ${type}`;
            messageDiv.textContent = text;
            chatbotMessages.appendChild(messageDiv);
            chatbotMessages.scrollTop = chatbotMessages.scrollHeight;
        }
    }
});

// Gestion des notifications
function showNotification(message, type = 'success') {
    const notification = document.createElement('div');
    notification.className = `notification ${type}`;
    notification.textContent = message;
    document.body.appendChild(notification);

    // Afficher la notification
    setTimeout(() => {
        notification.classList.add('show');
    }, 100);

    // Cacher et supprimer la notification après 5 secondes
    setTimeout(() => {
        notification.classList.remove('show');
        setTimeout(() => {
            notification.remove();
        }, 300);
    }, 5000);
}

// Gestion des formulaires
document.addEventListener('DOMContentLoaded', function() {
    // Validation des formulaires
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function(event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        });
    });

    // Mise à jour du nom du fichier sélectionné
    const fileInputs = document.querySelectorAll('.custom-file-input');
    fileInputs.forEach(input => {
        input.addEventListener('change', function() {
            const fileName = this.files[0]?.name || 'Choisir un fichier';
            this.nextElementSibling.textContent = fileName;
        });
    });
});

// Gestion des tables DataTables
$(document).ready(function() {
    if ($.fn.DataTable) {
        $('.table').DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.10.24/i18n/French.json'
            },
            responsive: true,
            pageLength: 25,
            order: [[3, 'desc']], // Trier par date de création par défaut
            columnDefs: [
                {
                    targets: -1,
                    orderable: false
                }
            ]
        });
    }
}); 