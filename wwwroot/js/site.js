// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// 🔹 Date and Time Display
function updateDateTime() {
    const now = new Date();
    const options = {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit',
        hour12: true
    };
    const dateTimeElement = document.getElementById("currentDateTime");
    if (dateTimeElement) {
        dateTimeElement.textContent = now.toLocaleString("en-PH", options);
    }
}
updateDateTime();
setInterval(updateDateTime, 1000);

// 🔹 Toggle password visibility (for the login form)
document.addEventListener("DOMContentLoaded", () => {
    const togglePassword = document.querySelector('#togglePassword');
    const password = document.querySelector('#password');
    if (togglePassword && password) {
        togglePassword.addEventListener('click', function () {
            const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
            password.setAttribute('type', type);
            this.classList.toggle('bi-eye');
            this.classList.toggle('bi-eye-fill');
        });
    }
});

function showResidentViewModal(residentName) {
    let modal = document.getElementById('residentModal');
    if (!modal) {
        modal = createResidentModal();
        document.body.appendChild(modal);
    }

    document.getElementById('modalTitle').textContent = 'VERIFY RESIDENT PROFILE';
    document.getElementById('verifyButtonContainer').style.display = 'none';

    modal.style.display = 'flex';
    document.body.style.overflow = 'hidden';
}

function showResidentVerifyModal(residentName) {
    let modal = document.getElementById('residentModal');
    if (!modal) {
        modal = createResidentModal();
        document.body.appendChild(modal);
    }

    document.getElementById('modalTitle').textContent = 'VERIFY RESIDENT PROFILE';
    document.getElementById('verifyButtonContainer').style.display = 'flex';

    modal.style.display = 'flex';
    document.body.style.overflow = 'hidden';
}

function closeResidentModal() {
    const modal = document.getElementById('residentModal');
    if (modal) {
        modal.style.display = 'none';
        document.body.style.overflow = 'auto';
    }
}

function createResidentModal() {
    const modal = document.createElement('div');
    modal.id = 'residentModal';
    modal.style.cssText = `
        display: none;
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.5);
        z-index: 9999;
        justify-content: center;
        align-items: center;
        padding: 20px;
    `;

    modal.innerHTML = `
        <div style="background-color: #f5f5dc; border-radius: 8px; width: 100%; max-width: 900px; max-height: 90vh; overflow-y: auto; position: relative; box-shadow: 0 4px 6px rgba(0,0,0,0.1);">
            <button id="closeResidentModal" style="position: absolute; top: 15px; right: 15px; background: none; border: none; font-size: 28px; cursor: pointer; color: #333; z-index: 10;">&times;</button>
            
            <div style="padding: 30px;">
                <h2 id="modalTitle" style="text-align: center; color: #6B8E23; margin-bottom: 30px; font-size: 24px; font-weight: bold;">VERIFY RESIDENT PROFILE</h2>
                
                <!-- I. Personal Information -->
                <div style="margin-bottom: 25px;">
                    <h4 style="color: #555; font-size: 16px; margin-bottom: 15px;">I. Personal Information</h4>
                    <div style="display: grid; grid-template-columns: repeat(4, 1fr); gap: 15px; margin-bottom: 15px;">
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">First Name</label>
                            <input type="text" value="JUAN" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Middle Name</label>
                            <input type="text" value="DELA" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Last Name</label>
                            <input type="text" value="CRUZ" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Suffix</label>
                            <input type="text" value="JR." readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                    </div>
                    <div style="display: grid; grid-template-columns: repeat(4, 1fr); gap: 15px; margin-bottom: 15px;">
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Date of Birth</label>
                            <input type="text" value="03-18-2005" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Place of Birth</label>
                            <input type="text" value="QUEZON CITY" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Civil Status</label>
                            <input type="text" value="SINGLE" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Sex</label>
                            <input type="text" value="MALE" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                    </div>
                    <div style="display: grid; grid-template-columns: repeat(2, 1fr); gap: 15px;">
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Contact No.</label>
                            <input type="text" value="09459971825" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Religion</label>
                            <input type="text" value="ROMAN CATHOLIC" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                    </div>
                </div>
                
                <!-- II. Residency Details -->
                <div style="margin-bottom: 25px;">
                    <h4 style="color: #555; font-size: 16px; margin-bottom: 15px;">II. Residency Details</h4>
                    <div style="display: grid; grid-template-columns: repeat(4, 1fr); gap: 15px;">
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">House No.</label>
                            <input type="text" value="D7" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Street Name</label>
                            <input type="text" value="SINFOROSA STREET" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Purok</label>
                            <input type="text" value="PUROK 1" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Residency Status</label>
                            <input type="text" value="RENTING" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                    </div>
                </div>
                
                <!-- III. Family and Household Details -->
                <div style="margin-bottom: 25px;">
                    <h4 style="color: #555; font-size: 16px; margin-bottom: 15px;">III. Family and Household Details</h4>
                    <div style="display: grid; grid-template-columns: repeat(4, 1fr); gap: 15px; margin-bottom: 15px;">
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">First Name</label>
                            <input type="text" value="JANE" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Middle Name</label>
                            <input type="text" value="GONZALES" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Last Name</label>
                            <input type="text" value="CRUZ" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Relationship</label>
                            <input type="text" value="MOTHER" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                    </div>
                    <div style="margin-bottom: 10px;">
                        <label style="display: block; color: #666; font-size: 13px; margin-bottom: 8px; font-weight: 500;">i. Head of the Household</label>
                    </div>
                    <div style="display: grid; grid-template-columns: repeat(4, 1fr); gap: 15px;">
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">First Name</label>
                            <input type="text" value="JOHN" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Middle Name</label>
                            <input type="text" value="SMITH" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Last Name</label>
                            <input type="text" value="CRUZ" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">No. of Dependents</label>
                            <input type="text" value="7" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                    </div>
                </div>
                
                <!-- IV. Employment Information -->
                <div style="margin-bottom: 25px;">
                    <h4 style="color: #555; font-size: 16px; margin-bottom: 15px;">IV. Employment Information</h4>
                    <div style="display: grid; grid-template-columns: repeat(4, 1fr); gap: 15px;">
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Educational Attainment</label>
                            <input type="text" value="COLLEGE GRADUATE" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Employment Status</label>
                            <input type="text" value="EMPLOYED" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Occupation</label>
                            <input type="text" value="CALL CENTER AGENT" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                        <div>
                            <label style="display: block; color: #888; font-size: 13px; margin-bottom: 5px;">Monthly Income</label>
                            <input type="text" value="12,000 - 20,000" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                        </div>
                    </div>
                </div>
                
                <!-- V. Other Information -->
                <div style="margin-bottom: 25px;">
                    <h4 style="color: #555; font-size: 16px; margin-bottom: 15px;">V. Other Information</h4>
                    <div style="margin-bottom: 15px;">
                        <label style="display: block; color: #666; font-size: 13px; margin-bottom: 8px; font-weight: 500;">i. Category</label>
                        <div style="display: flex; gap: 20px; align-items: center; margin-bottom: 10px;">
                            <label style="display: flex; align-items: center; gap: 5px; font-size: 13px;">
                                <input type="radio" checked disabled> Person with Disability (PWD)
                            </label>
                            <label style="display: flex; align-items: center; gap: 5px; font-size: 13px;">
                                <input type="radio" disabled> 4Ps
                            </label>
                            <label style="display: flex; align-items: center; gap: 5px; font-size: 13px;">
                                <input type="radio" disabled> Senior Citizen
                            </label>
                            <label style="display: flex; align-items: center; gap: 5px; font-size: 13px;">
                                <input type="radio" disabled> TUPAD Worker
                            </label>
                            <label style="display: flex; align-items: center; gap: 5px; font-size: 13px;">
                                <input type="radio" disabled> Solo Parent
                            </label>
                        </div>
                        <input type="text" value="MAJOR DEPRESSIVE DISORDER" readonly style="width: 100%; padding: 10px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                    </div>
                    
                    <div>
                        <label style="display: block; color: #666; font-size: 13px; margin-bottom: 8px; font-weight: 500;">ii. Miscellaneous</label>
                        <div style="display: grid; grid-template-columns: repeat(4, 1fr); gap: 20px;">
                            <div>
                                <label style="display: block; color: #888; font-size: 12px; margin-bottom: 5px;">Registered voter?</label>
                                <div style="display: flex; gap: 15px;">
                                    <label style="display: flex; align-items: center; gap: 5px; font-size: 13px;">
                                        <input type="radio" checked disabled> Yes
                                    </label>
                                    <label style="display: flex; align-items: center; gap: 5px; font-size: 13px;">
                                        <input type="radio" disabled> No
                                    </label>
                                </div>
                            </div>
                            <div>
                                <label style="display: block; color: #888; font-size: 12px; margin-bottom: 5px;">Financial Aid Recipient</label>
                                <div style="display: flex; gap: 15px;">
                                    <label style="display: flex; align-items: center; gap: 5px; font-size: 13px;">
                                        <input type="radio" disabled> Yes
                                    </label>
                                    <label style="display: flex; align-items: center; gap: 5px; font-size: 13px;">
                                        <input type="radio" checked disabled> No
                                    </label>
                                </div>
                            </div>
                            <div>
                                <label style="display: block; color: #888; font-size: 12px; margin-bottom: 5px;">Medical Aid Recipient</label>
                                <div style="display: flex; gap: 15px; margin-bottom: 5px;">
                                    <label style="display: flex; align-items: center; gap: 5px; font-size: 13px;">
                                        <input type="radio" checked disabled> Yes
                                    </label>
                                    <label style="display: flex; align-items: center; gap: 5px; font-size: 13px;">
                                        <input type="radio" disabled> No
                                    </label>
                                </div>
                                <input type="text" value="SSRI" readonly style="width: 100%; padding: 8px; background-color: #e8e8e8; border: none; border-radius: 4px; font-size: 13px;">
                            </div>
                            <div>
                                <label style="display: block; color: #888; font-size: 12px; margin-bottom: 5px;">Is resident an Informal Settler?</label>
                                <div style="display: flex; gap: 15px;">
                                    <label style="display: flex; align-items: center; gap: 5px; font-size: 13px;">
                                        <input type="radio" checked disabled> Yes
                                    </label>
                                    <label style="display: flex; align-items: center; gap: 5px; font-size: 13px;">
                                        <input type="radio" disabled> No
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <!-- Verify Button (conditionally shown) -->
                <div id="verifyButtonContainer" style="display: flex; justify-content: flex-end; margin-top: 20px;">
                    <button id="verifyNowBtn" style="padding: 12px 30px; background-color: #90c857; color: white; border: none; border-radius: 6px; font-size: 14px; cursor: pointer; font-weight: 500;">
                        Verify Now
                    </button>
                </div>
            </div>
        </div>
    `;

    return modal;
}